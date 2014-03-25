using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
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
        private readonly List<IPacket> _packets = new List<IPacket>();
        // -- Debugging.

        private delegate void DataReceived(int id, byte[] data);
        private event DataReceived OnDataReceived;

        public bool Connected {get { return _baseSock.Connected; }}

        private TcpClient _baseSock;
        private PacketStream _stream;
        private PacketByteReader _preader;

        private Thread _listener;
        private Thread _sender;

        private IMinecraft _minecraft;

        private readonly Queue<IPacket> _packetsToSend = new Queue<IPacket>();

        public NetworkHandler(IMinecraft client)
        {
            _minecraft = client;
        }

        /// <summary>
        ///     Starts the network handler.
        /// </summary>
        public bool Start()
        {
            // -- Connect to server.
            try
            {
                _baseSock = new TcpClient();
                _baseSock.Connect(_minecraft.ServerIP, _minecraft.ServerPort);
            }
            catch (SocketException) { return false; }

            // -- Create our Wrapped socket.
            _stream = new PacketStream(_baseSock.GetStream());

            // -- Subscribe to DataReceived event.
            OnDataReceived += HandlePacket;

            // -- Start network parsing.
            _listener = new Thread(StartReceiving) {Name = "PacketListener"};
            _listener.Start();

            // -- Start network sending.
            _sender = new Thread(StartSending) {Name = "PacketSender"};
            _sender.Start();

            return true;
        }

        #region Sending and Receiving.

        private void StartReceiving()
        {
            _preader = new PacketByteReader(new MemoryStream(0));

            do
            {
            } while (PacketReceiver());
        }

        private bool PacketReceiver()
        {
            if (_baseSock.Client == null || !Connected)
                return false;

            while (_baseSock.Client.Available > 0)
            {
                int length = _stream.ReadVarInt();
                int id = _stream.ReadVarInt();

                OnDataReceived(id, _stream.ReadByteArray(length - 1));
            }

            return true;
        }

        private void StartSending()
        {
            do
            {
            } while (PacketSender());
        }

        private bool PacketSender()
        {
            if (_baseSock.Client == null || !Connected)
                return false;

            if (_packetsToSend.Count == 0)
                return true;

            while (_packetsToSend.Count > 0)
            {
                Thread.Sleep(1); // -- Important to make a little pause.
                // _packetsToSend.Dequeue().WritePacket(ref _stream);
                IPacket packet = _packetsToSend.Dequeue();

                if (packet == null) 
                    continue;

                _packets.Add(packet);
                packet.WritePacket(ref _stream);

            }
            return true;
        }

        #endregion Sending and Receiving.

        private void HandlePacket(int id, byte[] data)
        {
            _preader.SetNewData(data);

            switch (_minecraft.State)
            {
                    #region Status

                case ServerState.Status:
                    if (ServerResponse.ServerStatusResponse[id] == null)
                        break;

                    IPacket packetS = ServerResponse.ServerStatusResponse[id]();
                    packetS.ReadPacket(_preader);
                    RaisePacketHandled(packetS, id, ServerState.Status);

                    break;

                    #endregion Status

                    #region Login

                case ServerState.Login:
                    if (ServerResponse.ServerLoginResponse[id] == null)
                        break;

                    IPacket packetL = ServerResponse.ServerLoginResponse[id]();
                    packetL.ReadPacket(_preader);
                    RaisePacketHandled(packetL, id, ServerState.Login);

                    if (id == 1)
                        EnableEncryption(packetL);

                    break;

                    #endregion Login

                    #region Play

                case ServerState.Play:
                    if (ServerResponse.ServerPlayResponse[id] == null)
                        break;

                    IPacket packetP = ServerResponse.ServerPlayResponse[id]();
                    packetP.ReadPacket(_preader);
                    RaisePacketHandled(packetP, id, ServerState.Play);

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

            byte[] hashData = hashlist.ToArray();

            string hash = Cryptography.JavaHexDigest(hashData);

            if (!Yggdrasil.ClientAuth(_minecraft.AccessToken, _minecraft.SelectedProfile, hash))
                return;

            // -- You pass it the key data and ask it to parse, and it will 
            // -- Extract the server's public key, then parse that into RSA for us.
            var keyParser = new AsnKeyParser(request.PublicKey);
            RSAParameters dekey = keyParser.ParseRSAPublicKey();

            // -- Now we create an encrypter, and encrypt the token sent to us by the server
            // -- as well as our newly made shared key (Which can then only be decrypted with the server's private key)
            // -- and we send it to the server.
            var cryptoService = new RSACryptoServiceProvider();
            cryptoService.ImportParameters(dekey);

            byte[] encryptedSecret = cryptoService.Encrypt(request.SharedKey, false);
            byte[] encryptedVerify = cryptoService.Encrypt(request.VerificationToken, false);

            _stream.InitEncryption(request.SharedKey);

            // Directly send packet because i have troubles with synchronizing "make EncEnabled after sending this packet".
            var packetToSend = new EncryptionResponsePacket
            {
                SharedSecret = encryptedSecret,
                VerificationToken = encryptedVerify
            };

            packetToSend.WritePacket(ref _stream);

            _stream.EncEnabled = true;
        }

        public void Send(IPacket packet)
        {
            _packetsToSend.Enqueue(packet);
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

            if (_preader != null)
                _preader.Dispose();

                _minecraft = null;
        }
    }
}