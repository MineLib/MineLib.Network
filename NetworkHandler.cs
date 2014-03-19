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
        public bool Connected {get { return _baseSock.Connected; }}

        private readonly List<IPacket> packets = new List<IPacket>(); // Debugging

        private TcpClient _baseSock;
        private NetworkStream _baseStream;
        private Thread _listener;
        private Thread _sender;
        private IMinecraft _minecraft;
        private PacketStream _stream;
        private PacketByteReader _preader;

        // Not using Queue because .Net 2.0
        private readonly List<IPacket> _packetsToSend = new List<IPacket>();

        public NetworkHandler(IMinecraft client)
        {
            _minecraft = client;
        }

        /// <summary>
        ///     Starts the network handler.
        /// </summary>
        public void Start()
        {
            try
            {
                _baseSock = new TcpClient();
                IAsyncResult AR = _baseSock.BeginConnect(_minecraft.ServerIP, _minecraft.ServerPort, null, null);
                WaitHandle wh = AR.AsyncWaitHandle;

                try
                {
                    if (!AR.AsyncWaitHandle.WaitOne(TimeSpan.FromSeconds(5), false))
                    {
                        _baseSock.Close();
                        // Failed to connect: Connection Timeout.
                        return;
                    }

                    _baseSock.EndConnect(AR);
                }
                finally
                {
                    wh.Close();
                }
            }
            catch (Exception) { return; } // Failed to connect: e.Message.

            // Connected to server.

            // -- Create our Wrapped socket.
            _baseStream = _baseSock.GetStream();
            _stream = new PacketStream(_baseStream);

            // Socket Created.

            // -- Start network parsing.
            _listener = new Thread(ReceiveUpdater) { Name = "PacketListener" };
            _listener.Start();

            _sender = new Thread(SendUpdater) { Name = "PacketSender" };
            _sender.Start();
            // Handler thread started.
        }

        /// <summary>
        ///     Stops and dispose the network handler.
        /// </summary>
        public void Stop()
        {
            Dispose();
        }

        private void ReceiveUpdater()
        {
            _preader = new PacketByteReader(new MemoryStream(512));
            try
            {
                do
                {
                } while (PacketReceiver());
            }
            catch (IOException) { }
            catch (SocketException) { }
            catch (ObjectDisposedException) { }
        }

        private void SendUpdater()
        {
            try
            {
                do
                {
                } while (PacketSender());
            }
            catch (IOException) { }
            catch (SocketException) { }
            catch (ObjectDisposedException) { }
        }

        /// <summary>
        ///     Creates an instance of each new packet, so it can be parsed.
        /// </summary>
        private bool PacketReceiver()
        {
            try
            {
                if (_baseSock.Client == null || !_baseSock.Connected)
                    return false;

                while (_baseSock.Client.Available > 0)
                {
                    int length = _stream.ReadVarInt();
                    int packetID = _stream.ReadVarInt();

                    HandlePacket(packetID, _stream.ReadByteArray(length - 1));
                }
            }
            catch (SocketException)
            {
                Console.WriteLine("Error");
                //Stop();
                return false;
            }
            return true;
        }

        private bool PacketSender()
        {
            if (_packetsToSend.Count == 0)
                return true;

            while (_packetsToSend.Count > 0)
            {
                IPacket packet = _packetsToSend[0];
                _packetsToSend.RemoveAt(0);

                if (packet == null) 
                    continue;

                packets.Add(packet);
                packet.WritePacket(ref _stream);

            }
            return true;
        }

        private void HandlePacket(int packetID, byte[] data)
        {
            _preader.SetNewData(data);

            switch (_minecraft.State)
            {
                    #region Status

                case ServerState.Status:
                    if (ServerResponse.ServerStatusResponse[packetID] == null)
                        break;

                    IPacket packetS = ServerResponse.ServerStatusResponse[packetID]();
                    packetS.ReadPacket(_preader);
                    RaisePacketHandled(packetS, packetID, ServerState.Status);

                    break;

                    #endregion Status

                    #region Login

                case ServerState.Login:
                    if (ServerResponse.ServerLoginResponse[packetID] == null)
                        break;

                    IPacket packetL = ServerResponse.ServerLoginResponse[packetID]();
                    packetL.ReadPacket(_preader);
                    RaisePacketHandled(packetL, packetID, ServerState.Login);

                    if (packetID == 1)
                        EnableEncryption(packetL);

                    break;

                    #endregion Login

                    #region Play

                case ServerState.Play:
                    if (ServerResponse.ServerPlayResponse[packetID] == null)
                        break;

                    IPacket packetP = ServerResponse.ServerPlayResponse[packetID]();
                    packetP.ReadPacket(_preader);
                    RaisePacketHandled(packetP, packetID, ServerState.Play);

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
            _packetsToSend.Add(packet);
        }

        public void Dispose()
        {
            if (_listener.IsAlive)
                _listener.Abort();

            if (_baseSock != null)
                _baseSock.Close();

            if (_baseStream != null)
                _baseStream.Dispose();

            if (_stream != null)
                _stream.Dispose();

                _minecraft = null;
        }
    }
}