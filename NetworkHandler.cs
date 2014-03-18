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

        private TcpClient _baseSock;
        private NetworkStream _baseStream;
        private Thread _listener;
        private IMinecraft _minecraft;
        private Wrapped _stream;

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
            _stream = new Wrapped(_baseStream);

            // Socket Created.

            // -- Start network parsing.
            _listener = new Thread(Updater) {Name = "PacketListener"};
            _listener.Start();
            // Handler thread started.
        }

        /// <summary>
        ///     Stops and dispose the network handler.
        /// </summary>
        public void Stop()
        {
            Dispose();
        }

        private void Updater()
        {
            try
            {
                do
                {
                } while (PacketHandler());
            }
            catch (IOException) {}
            catch (SocketException) {}
            catch (ObjectDisposedException) {}
        }

        /// <summary>
        ///     Creates an instance of each new packet, so it can be parsed.
        /// </summary>
        private bool PacketHandler()
        {
            try
            {
                if (_baseSock.Client == null || !_baseSock.Connected)
                    return false;

                while (_baseSock.Client.Available > 0)
                {
                    Console.WriteLine("In While");

                    int length = _stream.ReadVarInt();
                    int packetID = _stream.ReadVarInt();

                    Console.WriteLine("ID : 0x" + String.Format("{0:X}", packetID));
                    Console.WriteLine("Lenght: " + length);

                    switch (_minecraft.State)
                    {
                            #region Status

                        case ServerState.Status:
                            if (ServerResponse.ServerStatusResponse[packetID] == null)
                            {
                                _stream.ReadByteArray(length - 1); // -- bypass the packet
                                break;
                            }

                            HandlePacket(_stream.ReadByteArray(length - 1), packetID, ServerState.Status);

                            break;

                            #endregion Status

                            #region Login

                            // We handle it right here.
                        case ServerState.Login:
                            if (ServerResponse.ServerLoginResponse[packetID] == null)
                            {
                                _stream.ReadByteArray(length - 1); // -- bypass the packet
                                break;
                            }

                            IPacket packetL = ServerResponse.ServerLoginResponse[packetID]();
                            packetL.ReadPacket(ref _stream);
                            RaisePacketHandled(packetL, packetID, ServerState.Login);

                            break;

                            #endregion Login

                            #region Play

                        case ServerState.Play:
                            if (ServerResponse.ServerPlayResponse[packetID] == null)
                            {
                                _stream.ReadByteArray(length - 1); // -- bypass the packet
                                break;
                            }

                            HandlePacket(_stream.ReadByteArray(length - 1), packetID, ServerState.Play);

                            break;

                            #endregion Play
                    }
                    Console.WriteLine("Out While");
                    Console.WriteLine(" ");
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

        private void HandlePacket(byte[] data, int packetID, ServerState state)
        {
            var stream = new MemoryStream(data);
            var wrapped = new Wrapped(stream);

            switch (state)
            {
                case ServerState.Login: // We don't parse here Login, but others may do that here.
                    IPacket packetL = ServerResponse.ServerStatusResponse[packetID]();
                    packetL.ReadPacket(ref wrapped);
                    RaisePacketHandled(packetL, packetID, ServerState.Status);

                    break;

                  case  ServerState.Status:
                    IPacket packetS = ServerResponse.ServerStatusResponse[packetID]();
                    packetS.ReadPacket(ref wrapped);
                    RaisePacketHandled(packetS, packetID, ServerState.Status);

                    break;

                  case ServerState.Play:
                    IPacket packetP = ServerResponse.ServerPlayResponse[packetID]();
                    packetP.ReadPacket(ref wrapped);
                    RaisePacketHandled(packetP, packetID, ServerState.Play);

                    break;
            }

        }

        public void EnableEncryption(IPacket packet)
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

            Send(new EncryptionResponsePacket {SharedSecret = encryptedSecret, VerificationToken = encryptedVerify});

            _stream.EncEnabled = true;
        }

        public void Send(IPacket packet)
        {
            packet.WritePacket(ref _stream);
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