using System;
using System.IO;
using System.Net.Sockets;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
using MineLib.Network.IO;
using MineLib.Network.Modern;
using MineLib.Network.Modern.Packets;
using MineLib.Network.Modern.Packets.Client.Login;
using MineLib.Network.Modern.Packets.Server.Login;

namespace MineLib.Network
{
    // -- Modern logic is stored here.
    public sealed partial class NetworkHandler
    {
        public bool CompressionEnabled { get { return _stream.ModernCompressionEnabled; } }
        public long CompressionThreshold { get { return _stream.ModernCompressionThreshold; } }


        private void ConnectedModern(IAsyncResult asyncResult)
        {
            _baseSock.EndConnect(asyncResult);

            // -- Create our Wrapped socket.
            _stream = new MinecraftStream(new NetworkStream(_baseSock), NetworkMode);

            // -- Subscribe to DataReceived event.
            OnDataReceived += HandlePacketModern;

            // -- Begin data reading.
            _stream.BeginRead(new byte[0], 0, 0, PacketReceiverModernAsync, null);
        }

        private void PacketReceiverModernAsync(IAsyncResult ar)
        {
            if (_baseSock == null || _stream == null || !Connected || Crashed)
                return; // -- Terminate cycle

            if (_baseSock.Available > 0)
            {
                int packetId = 0;
                byte[] data = new byte[0];

                #region No Compression

                if (!CompressionEnabled)
                {
                    var packetLength = _stream.ReadVarInt();
                    if (packetLength == 0)
                        throw new Exception("Reading Error: Packet Length size is 0");

                    packetId = _stream.ReadVarInt();

                    data = _stream.ReadByteArray(packetLength - 1);
                }

                #endregion

                #region Compression

                else // (CompressionEnabled)
                {
                    var packetLength = _stream.ReadVarInt();
                    if (packetLength == 0)
                        throw new Exception("Reading Error: Packet Length size is 0");

                    var dataLength = _stream.ReadVarInt();
                    if (dataLength == 0)
                    {
                        if (packetLength >= CompressionThreshold)
                            throw new Exception("Reading Error: Received uncompressed message of size " + packetLength +
                                                " greater than threshold " + CompressionThreshold);

                        packetId = _stream.ReadVarInt();

                        data = _stream.ReadByteArray(packetLength - 2);
                    }
                    else // (dataLength > 0)
                    {
                        var dataLengthBytes = MinecraftStream.GetVarIntBytes(dataLength).Length;

                        var tempBuff = _stream.ReadByteArray(packetLength - dataLengthBytes); // -- Compressed

                        using (var outputStream = new MemoryStream())
                        using (var inputStream = new InflaterInputStream(new MemoryStream(tempBuff)))
                            //using (var reader = new MinecraftDataReader(new MemoryStream(tempBuff), NetworkMode))
                        {
                            inputStream.CopyTo(outputStream);
                            tempBuff = outputStream.ToArray(); // -- Decompressed

                            packetId = tempBuff[0]; // -- Only 255 packets available. ReadVarInt doesn't work.
                            var packetIdBytes = MinecraftStream.GetVarIntBytes(packetId).Length;

                            data = new byte[tempBuff.Length - packetIdBytes];
                            Buffer.BlockCopy(tempBuff, packetIdBytes, data, 0, data.Length);
                        }
                    }
                }

                #endregion

                OnDataReceived(packetId, data);
            }

            // -- If it will throw an error, then the cause is too slow _stream dispose
            _stream.EndRead(ar);
            _stream.BeginRead(new byte[0], 0, 0, PacketReceiverModernAsync, null);
        }

        /// <summary>
        /// Packets are handled here. Compression and encryption are handled here too
        /// </summary>
        /// <param name="id">Packet ID</param>
        /// <param name="data">Packet byte[] data</param>
        private void HandlePacketModern(int id, byte[] data)
        {
            using (var reader = new MinecraftDataReader(data, NetworkMode))
            {
                IPacket packet;

                switch (_minecraft.State)
                {
                    #region Status

                    case ServerState.ModernStatus:
                        if (ServerResponse.Status[id] == null)
                            break;

                        packet = ServerResponse.Status[id]().ReadPacket(reader);

                        RaisePacketHandled(id, packet, ServerState.ModernStatus);

                        break;

                    #endregion Status

                    #region Login

                    case ServerState.ModernLogin:
                        if (ServerResponse.Login[id] == null)
                            break;

                        packet = ServerResponse.Login[id]().ReadPacket(reader);

                        RaisePacketHandled(id, packet, ServerState.ModernLogin);

                        if (id == 0x01)
                            ModernEnableEncryption(packet);  // -- Low-level encryption handle

                        if (id == 0x03)
                            ModernSetCompression(packet); // -- Low-level compression handle

                        break;

                    #endregion Login

                    #region Play

                    case ServerState.ModernPlay:
                        if (ServerResponse.Play[id] == null)
                            break;

                        packet = ServerResponse.Play[id]().ReadPacket(reader);

                        if (ServerResponse.Play[id]() == null)
                            throw new Exception("");

                        RaisePacketHandled(id, packet, ServerState.ModernPlay);

                        if (id == 0x46)
                            ModernSetCompression(packet); // -- Low-level compression handle

                        // Connection lost
                        if (id == 0x40)
                            Crashed = true;

                        break;

                    #endregion Play
                }
            }
        }

        /// <summary>
        /// Enabling encryption
        /// </summary>
        /// <param name="packet">EncryptionRequestPacket</param>
        private void ModernEnableEncryption(IPacket packet)
        {
            var request = (EncryptionRequestPacket)packet;

            var hash = Asn1.GetServerIDHash(request.PublicKey, request.SharedKey, request.ServerId);

            if (!Yggdrasil.ClientAuth(_minecraft.AccessToken, _minecraft.SelectedProfile, hash))
                throw new Exception("Yggdrasil error: Not authenticated.");

            var rsaParameter = Asn1.GetRsaParameters(request.PublicKey);

            var encryptedSecret = Asn1.EncryptData(rsaParameter, request.SharedKey);
            var encryptedVerify = Asn1.EncryptData(rsaParameter, request.VerificationToken);

            BeginSendPacket(new EncryptionResponsePacket
            {
                SharedSecret = encryptedSecret,
                VerificationToken = encryptedVerify
            }, null, null);

            _stream.InitializeEncryption(request.SharedKey);
        }

        /// <summary>
        /// Setting compression
        /// </summary>
        /// <param name="packet">EncryptionRequestPacket</param>
        private void ModernSetCompression(IPacket packet)
        {
            var request = (ISetCompression) packet;

            _stream.SetCompression(request.Threshold);
        }
    }
}
