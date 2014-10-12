using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
using MineLib.Network.Cryptography;
using MineLib.Network.IO;
using MineLib.Network.Main.Packets;
using MineLib.Network.Main.Packets.Client.Login;
using MineLib.Network.Main.Packets.Server.Login;

namespace MineLib.Network
{
    public partial class NetworkHandler
    {
        public bool CompressionEnabled { get { return _stream.CompressionEnabled; } }
        public int CompressionThreshold { get { return _stream.CompressionThreshold; } }


        private void StartReceivingMainSync()
        {
            try
            {
                do
                {
                    Thread.Sleep(50);
                } while (PacketReceiverMainSync());
            }
            catch (SocketException)
            {
                Crashed = true;
            }
        }

        private bool PacketReceiverMainSync()
        {
            if (_baseSock == null || !Connected)
                return false;

            while (_baseSock.Available > 0)
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

                        var data = new byte[tempBuff.Length - 1];
                        Buffer.BlockCopy(tempBuff, 1, data, 0, data.Length);

                        OnDataReceived(id, data);
                    }
                }
            }

            return true;
        }

        private void PacketReceiverMainAsync(IAsyncResult ar)
        {
            if (_baseSock == null || !Connected)
                return;

            int packetId;
            byte[] data;

            if (!CompressionEnabled)
            {
                var packetLength = _stream.ReadVarInt();
                packetId = _stream.ReadVarInt();
                data = _stream.ReadByteArray(packetLength - 1);
            }
            else
            {
                var packetLength = _stream.ReadVarInt();
                var dataLength = _stream.ReadVarInt();
                if (dataLength == 0)
                {
                    if (packetLength >= CompressionThreshold)
                        throw new Exception("Received uncompressed message of size " + packetLength +
                                            " greater than threshold " + CompressionThreshold);

                    packetId = _stream.ReadVarInt();

                    var packetLengthBytes = PacketStream.GetVarIntBytes(packetLength).Length;
                    var dataLengthBytes = PacketStream.GetVarIntBytes(dataLength).Length;
                    var t = packetLengthBytes + dataLengthBytes;
                    data = _stream.ReadByteArray(packetLength - 2);
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

                    packetId = tempBuff[0]; // TODO: Will be broken when ID will be more than 256.

                    data = new byte[tempBuff.Length - 1];
                    Buffer.BlockCopy(tempBuff, 1, data, 0, data.Length);
                }
            }

            OnDataReceived(packetId, data);

            _baseSock.EndReceive(ar);
            _baseSock.BeginReceive(new byte[0], 0, 0, SocketFlags.None, PacketReceiverMainAsync, _baseSock);
        }


        /// <summary>
        /// Packets are handled here. Compression and encryption are handled here too
        /// </summary>
        /// <param name="id">Packet ID</param>
        /// <param name="data">Packet byte[] data</param>
        private void HandlePacketMain(int id, byte[] data)
        {
            _reader = new PacketByteReader(data, Mode);
            IPacket packet;

            switch (_minecraft.State)
            {
                #region Status

                case ServerState.MainStatus:
                    if (ServerResponse.Status[id] == null)
                        break;

                    packet = ServerResponse.Status[id]();
                    packet.ReadPacket(_reader);

                    RaisePacketHandledMain(id, packet, ServerState.MainStatus);

                    break;

                #endregion Status

                #region Login

                case ServerState.MainLogin:
                    if (ServerResponse.Login[id] == null)
                        break;

                    packet = ServerResponse.Login[id]();
                    packet.ReadPacket(_reader);

                    RaisePacketHandledMain(id, packet, ServerState.MainLogin);

                    if (id == 0x01)
                        EnableEncryption(packet);  // -- Low-level encryption handle

                    if (id == 0x03)
                        SetCompression(packet); // -- Low-level compression handle

                    break;

                #endregion Login

                #region Play

                case ServerState.MainPlay:
                    if (ServerResponse.Play[id] == null)
                        break;

                    packet = ServerResponse.Play[id]();
                    packet.ReadPacket(_reader);

                    RaisePacketHandledMain(id, packet, ServerState.MainPlay);

                    if (id == 0x46)
                        SetCompression(packet); // -- Low-level compression handle

                    // Connection lost
                    if (id == 0x40)
                    {
                        Crashed = true;
                        return;
                    }

                    break;

                #endregion Play
            }
        }

        private void EnableEncryption(IPacket packet)
        {
            // From libMC.NET
            var request = (EncryptionRequestPacket)packet;

            var hashlist = new List<byte>();
            hashlist.AddRange(Encoding.ASCII.GetBytes(request.ServerId));
            hashlist.AddRange(request.SharedKey);
            hashlist.AddRange(request.PublicKey);

            var hashData = hashlist.ToArray();

            var hash = JavaHelper.JavaHexDigest(hashData);

            if (!Yggdrasil.ClientAuth(_minecraft.AccessToken, _minecraft.SelectedProfile, hash))
            {
                throw new Exception("Auth failure");
                return;
            }

            // -- You pass it the key data and ask it to parse, and it will 
            // -- Extract the server's public key, then parse that into RSA for us.
            var keyParser = new AsnKeyParser(request.PublicKey);
            var deKey = keyParser.ParseRSAPublicKey();

            // -- Now we create an encrypter, and encrypt the token sent to us by the server
            // -- as well as our newly made shared key (Which can then only be decrypted with the server's private key)
            // -- and we send it to the server.
            var cryptoService = new RSACryptoServiceProvider();
            cryptoService.ImportParameters(deKey);

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
            var request = (ISetCompression)packet;

            _stream.SetCompression(request.Threshold);
        }
    }
}
