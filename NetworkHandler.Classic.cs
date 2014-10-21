using System;
using System.Net.Sockets;
using MineLib.Network.Classic.Packets;
using MineLib.Network.IO;

namespace MineLib.Network
{
    public sealed partial class NetworkHandler
    {
        private void PacketReceiverClassicAsync(IAsyncResult ar)
        {
            if (_baseSock == null || !Connected)
                return;

            var packetId = _stream.ReadByte();

            // Connection lost
            if (packetId == 255)
            {
                Crashed = true;
                return;
            }

            var length = ServerResponseClassic.ServerResponse[packetId]().Size;
            var data = _stream.ReadByteArray(length - 1);

            OnDataReceived(packetId, data);

            _baseSock.EndReceive(ar);
            _baseSock.BeginReceive(new byte[0], 0, 0, SocketFlags.None, PacketReceiverClassicAsync, null);
        }
        
        /// <summary>
        /// Packets are handled here.
        /// </summary>
        /// <param name="id">Packet ID</param>
        /// <param name="data">Packet byte[] data</param>
        private void HandlePacketClassic(int id, byte[] data)
        {
            var reader = new MinecraftDataReader(data, NetworkMode);

            if (ServerResponseClassic.ServerResponse[id] == null)
                return;

            var packet = ServerResponseClassic.ServerResponse[id]().ReadPacket(reader);

            RaisePacketHandled(id, packet, null);
        }
    }
}
