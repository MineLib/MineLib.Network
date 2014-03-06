using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Threading;
using CWrapped;
using MineLib.Network.Enums;
using MineLib.Network.Packets;
using MineLib.Network.Packets.Client;
using MineLib.Network.Packets.Client.Login;
using MineLib.Network.Packets.Server;
using MineLib.Network.Packets.Server.Login;

namespace MineLib.Network
{
    public partial class NetworkHandler : IDisposable
    {
        private Thread _handler;
        private IMinecraft _minecraft;
        private TcpClient _baseSock;
        private NetworkStream _baseStream;
        private Wrapped _stream;

        public NetworkHandler(IMinecraft client)
        {
            _minecraft = client;
        }

        /// <summary>
        /// Starts the network handler.
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
            catch (Exception e) { return; } // Failed to connect: e.Message.

            _minecraft.Running = true;

            // Connected to server.

            // -- Create our Wrapped socket.
            _baseStream = _baseSock.GetStream();
            _stream = new Wrapped(_baseStream);

            // Socket Created

            // -- Start network parsing.
            _handler = new Thread(Updater);
            _handler.Start();
            // Handler thread started.
        }

        /// <summary>
        /// Stops and dispose the network handler.
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
                    Thread.Sleep(100);
                } while (PacketHandler());
            }
            catch (IOException) {}
            catch (SocketException) {}
            catch (ObjectDisposedException) {}
        }

        /// <summary>
        /// Creates an instance of each new packet, so it can be parsed.
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
                        case ServerState.Status:
                            if (ServerResponse.ServerStatusResponse[packetID] == null)
                            {
                                _stream.ReadByteArray(length - 1); // -- bypass the packet
                                continue;
                            }

                            var packetS = ServerResponse.ServerStatusResponse[packetID]();
                            packetS.ReadPacket(ref _stream);
                            RaisePacketHandled(this, packetS, packetID, ServerState.Status);

                            break;

                        case ServerState.Login:
                            if (ServerResponse.ServerLoginResponse[packetID] == null)
                            {
                                _stream.ReadByteArray(length - 1); // -- bypass the packet
                                continue;
                            }

                            var packetL = ServerResponse.ServerLoginResponse[packetID]();
                            packetL.ReadPacket(ref _stream);
                            RaisePacketHandled(this, packetL, packetID, ServerState.Login);

                            if (packetID == 1)
                                EnableEncryption(packetL);

                            if (packetID == 2)
                                _minecraft.State = ServerState.Play;
                            

                            if (packetID == 3) //Shit just got real.
                            Stop();

                            break;

                        case ServerState.Play:
                            if (ServerResponse.ServerPlayResponse[packetID] == null)
                            {
                                _stream.ReadByteArray(length - 1); // -- bypass the packet
                                continue;
                            }

                            var packetP = ServerResponse.ServerPlayResponse[packetID]();
                            packetP.ReadPacket(ref _stream);
                            RaisePacketHandled(this, packetP, packetID, ServerState.Play);

                            break;
                    }
                    Console.WriteLine("Out While");
                    Console.WriteLine(" ");
                }
            }
            catch (SocketException e)
            {
                Console.WriteLine("Error");
                //Stop();
                return false;
            }
            return true;
        }

        private void EnableEncryption(IPacket packet)
        {
            // From libMC.NET
            EncryptionRequestPacket Request = (EncryptionRequestPacket) packet;

            List<byte> hashlist = new List<byte>();
            hashlist.AddRange(System.Text.Encoding.ASCII.GetBytes(Request.ServerId));
            hashlist.AddRange(Request.SharedKey);
            hashlist.AddRange(Request.PublicKey);

            byte[] hashData = hashlist.ToArray();

            string hash = Cryptography.JavaHexDigest(hashData);

            if (!Yggdrasil.VerifyName(_minecraft.AccessToken, _minecraft.SelectedProfile, hash))
                return;

            // -- AsnKeyParser is a part of the cryptography.dll, which is simply a compiled version
            // -- of SMProxy's Cryptography.cs, with the server side parts stripped out.
            // -- You pass it the key data and ask it to parse, and it will 
            // -- Extract the server's public key, then parse that into RSA for us.

            AsnKeyParser keyParser = new AsnKeyParser(Request.PublicKey);
            RSAParameters dekey = keyParser.ParseRSAPublicKey();

            // -- Now we create an encrypter, and encrypt the token sent to us by the server
            // -- as well as our newly made shared key (Which can then only be decrypted with the server's private key)
            // -- and we send it to the server.

            RSACryptoServiceProvider cryptoService = new RSACryptoServiceProvider();
            cryptoService.ImportParameters(dekey);

            byte[] EncryptedSecret = cryptoService.Encrypt(Request.SharedKey, false);
            byte[] EncryptedVerify = cryptoService.Encrypt(Request.VerificationToken, false);

            _stream.InitEncryption(Request.SharedKey);

            Send(new EncryptionResponsePacket {SharedSecret = EncryptedSecret, VerificationToken = EncryptedVerify});

            _stream.EncEnabled = true;

        }

        public void Send(IPacket packet)
        {
            packet.WritePacket(ref _stream);
        }

        public void Dispose()
        {
            if (_handler.IsAlive)
                _handler.Abort();

            if (_baseSock != null)
                _baseSock.Close();

            if (_baseStream != null)
                _baseStream.Dispose();

            if (_stream != null)
                _stream.Dispose();

            if (_minecraft != null)
                _minecraft = null;
        }
    }
}
