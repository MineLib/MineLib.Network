using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
using MineLib.Network.Cryptography;
using MineLib.Network.IO;
using MineLib.Network.Enums;
using MineLib.Network.Packets;
using MineLib.Network.Packets.Client.Login;
using MineLib.Network.Packets.Server.Login;

namespace MineLib.Network
{
    public partial class NetworkHandler : IDisposable
    {
        // -- Debugging
        private readonly List<IPacket> _packetsReceived = new List<IPacket>();
        private readonly List<IPacket> _packetsSended = new List<IPacket>();
        // -- Debugging.

        private delegate void DataReceived(int id, byte[] data);
        private event DataReceived OnDataReceived;
        private event DataReceived OnDataReceivedClassic;

        public bool Connected { get { return _baseSock.Connected; }}

        public bool CompressionEnabled { get { return _stream.CompressionEnabled; }}
        public int CompressionThreshold { get { return _stream.CompressionThreshold; } }

        public bool Crashed;

        public bool Classic { get; set; }

        private TcpClient _baseSock;
        private PacketStream _stream;
        private PacketByteReader _reader;

        private Thread _listener;
        private Thread _sender;

        private IMinecraftClient _minecraft;

        private readonly BlockingCollection<IPacket> _packetsToSend = new BlockingCollection<IPacket>();


        public NetworkHandler(IMinecraftClient client)
        {
            _minecraft = client;
            Crashed = false;
        }

        /// <summary>
        ///     Starts the network handler.
        /// </summary>
        public void Start()
        {
            // -- Connect to server.
            try
            {
                _baseSock = new TcpClient();
                _baseSock.Connect(_minecraft.ServerHost, _minecraft.ServerPort);

            }
            catch (SocketException)
            {
                Crashed = true;
                return;
            }

            // -- Create our Wrapped socket.
            _stream = new PacketStream(_baseSock.GetStream());

            // -- Subscribe to DataReceived event.
            OnDataReceived += HandlePacket;
            OnDataReceivedClassic += HandlePacketClassic;

            // -- Start network parsing.
            _listener = Classic
                ? new Thread(StartReceivingClassic) { Name = "PacketListenerClassic" }
                : new Thread(StartReceiving) {Name = "PacketListener"};
            _listener.Start();

            // -- Start network sending.
            _sender = Classic
                ? new Thread(StartSendingClassic) { Name = "PacketSenderClassic" }
                : new Thread(StartSending) { Name = "PacketSender" };
            _sender.Start();
        }

        #region Sending and Receiving.

        private void StartReceiving()
        {
            _reader = new PacketByteReader(new MemoryStream(0));

            try
            {
                do
                {
                } while (PacketReceiver());
            }
            catch (SocketException)
            {
                Crashed = true;
            }
        }

        private bool PacketReceiver()
        {
            if (_baseSock.Client == null || !Connected)
                return false;

            while (_baseSock.Client.Available > 0)
            {
                if (!CompressionEnabled)
                {
                    var length = _stream.ReadVarInt();
                    var id = _stream.ReadVarInt();

                    OnDataReceived(id, _stream.ReadByteArray(length - 1));
                }
                else
                {
                    var packetLength = _stream.ReadVarInt();
                    var dataLength = _stream.ReadVarInt();
                    int id = 0;

                    if (dataLength == 0) 
                    {
                        if (packetLength >= CompressionThreshold)
                            throw new Exception("Received uncompressed message of size " + packetLength + " greater than threshold " + CompressionThreshold);

                        id = _stream.ReadVarInt();

                        var packetLengthBytes = PacketStream.GetVarIntBytes(packetLength).Length;
                        var dataLengthBytes = PacketStream.GetVarIntBytes(dataLength).Length;
                        var t = packetLengthBytes + dataLengthBytes;
                        OnDataReceived(id, _stream.ReadByteArray(packetLength - 2)); // TODO: What is 2 here? (packetLength - packetLengthBytes - dataLengthBytes)?
                    }
                    else // (dataLength > 0)
                    {
                        var dataLengthBytes = PacketStream.GetVarIntBytes(dataLength).Length;

                        var tempBuff = _stream.ReadByteArray(packetLength - dataLengthBytes);

                        using (var outputStream = new MemoryStream())
                        using (var inputStream = new InflaterInputStream(new MemoryStream(tempBuff)))
                        {
                            inputStream.CopyTo(outputStream);
                            tempBuff = outputStream.ToArray();
                        }

                        id = tempBuff[0]; // TODO: Will be broken when ID will be more than 256.

                        var data = new byte[tempBuff.Length -  1];
                        Buffer.BlockCopy(tempBuff, 1, data, 0, data.Length);

                        OnDataReceived(id, data);
                    }
                }
            }

            return true;
        }

        private void StartSending()
        {
            try
            {
                do
                {
                } while (PacketSender());
            }
            catch (SocketException)
            {
                Crashed = true;
            }
            catch (IOException)
            {
                Crashed = true;
                //throw new Exception("Connection lost.");
            }
        }

        private bool PacketSender()
        {
            if (_baseSock.Client == null || !Connected)
                return false;

            if (_packetsToSend.Count == 0)
                return true;

            while (_packetsToSend.Count > 0)
            {                  
                var packet = _packetsToSend.Take();

#if DEBUG
                _packetsSended.Add(packet);
#endif

                packet.WritePacket(ref _stream);
            }

            return true;
        }

        #endregion Sending and Receiving.

        /// <summary>
        /// Packets are handled here. Compression and encryption are handled here too
        /// </summary>
        /// <param name="id">Packet ID</param>
        /// <param name="data">Packet byte[] data</param>
        private void HandlePacket(int id, byte[] data)
        {
            _reader = new PacketByteReader(data);
            IPacket packet;

            switch (_minecraft.State)
            {
                    #region Status

                case ServerState.Status:
                    if (ServerResponse.Status[id] == null)
                        break;

                    packet = ServerResponse.Status[id]();
                    packet.ReadPacket(_reader);

#if DEBUG
                    _packetsReceived.Add(packet);
#endif

                    RaisePacketHandled(packet, id, ServerState.Status);

                    break;

                    #endregion Status

                    #region Login

                case ServerState.Login:
                    if (ServerResponse.Login[id] == null)
                        break;

                    packet = ServerResponse.Login[id]();
                    packet.ReadPacket(_reader);

#if DEBUG
                    _packetsReceived.Add(packet);
#endif

                    RaisePacketHandled(packet, id, ServerState.Login);

                    if (id == 1)
                        EnableEncryption(packet);  // -- Low-level encryption handle

                    if (id == 3)
                        SetCompression(packet); // -- Low-level compression handle

                    break;

                    #endregion Login

                    #region Play

                case ServerState.Play:
                    if (ServerResponse.Play[id] == null)
                        break;

                    packet = ServerResponse.Play[id]();
                    packet.ReadPacket(_reader);

#if DEBUG
                    _packetsReceived.Add(packet);
#endif

                    RaisePacketHandled(packet, id, ServerState.Play);

                    if (id == 70)
                        SetCompression(packet); // -- Low-level compression handle

                    break;

                    #endregion Play
            }
        }

        private void EnableEncryption(IPacket packet)
        {
            // From libMC.NET
            var request = (EncryptionRequestPacket) packet;

            var hashlist = new List<byte>();
            hashlist.AddRange(Encoding.ASCII.GetBytes(request.ServerId));
            hashlist.AddRange(request.SharedKey);
            hashlist.AddRange(request.PublicKey);

            var hashData = hashlist.ToArray();

            var hash = JavaHelper.JavaHexDigest(hashData);

            if (!Yggdrasil.ClientAuth(_minecraft.AccessToken, _minecraft.SelectedProfile, hash))
                return;

            // -- You pass it the key data and ask it to parse, and it will 
            // -- Extract the server's public key, then parse that into RSA for us.
            var keyParser = new AsnKeyParser(request.PublicKey);
            var dekey = keyParser.ParseRSAPublicKey();

            // -- Now we create an encrypter, and encrypt the token sent to us by the server
            // -- as well as our newly made shared key (Which can then only be decrypted with the server's private key)
            // -- and we send it to the server.
            var cryptoService = new RSACryptoServiceProvider();
            cryptoService.ImportParameters(dekey);

            var encryptedSecret = cryptoService.Encrypt(request.SharedKey, false);
            var encryptedVerify = cryptoService.Encrypt(request.VerificationToken, false);

            _stream.InitializeEncryption(request.SharedKey);

            // Directly send packet because i have troubles with synchronizing "make EncEnabled after sending this packet".
            var packetToSend = new EncryptionResponsePacket
            {
                SharedSecret = encryptedSecret,
                VerificationToken = encryptedVerify
            };

            packetToSend.WritePacket(ref _stream);

            _stream.EncryptionEnabled = true;
        }

        private void SetCompression(IPacket packet)
        {
            var request = (ISetCompression) packet;

            _stream.SetCompression(request.Threshold);
        }

        public void Send(IPacket packet)
        {
            _packetsToSend.Add(packet);
        }

        /// <summary>
        ///     Stops and dispose the network handler.
        /// </summary>
        public void Dispose()
        {
            if (_listener != null && _listener.IsAlive)
                _listener.Abort();

            if (_sender != null && _sender.IsAlive)
                _sender.Abort();

            if (_baseSock != null)
                _baseSock.Close();

            if (_stream != null)
                _stream.Dispose();

            if (_reader != null)
                _reader.Dispose();

                _minecraft = null;
        }
    }
}