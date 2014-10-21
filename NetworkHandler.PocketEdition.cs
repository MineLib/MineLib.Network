using System;
using System.Net.Sockets;
using MineLib.Network.IO;
using MineLib.Network.PocketEdition.Packets;

namespace MineLib.Network
{
    public sealed partial class NetworkHandler
    {
        private void PacketReceiverPocketEditionAsync(IAsyncResult ar)
        {
            if (_baseSock == null || !Connected)
                return;

            var packetId = _stream.ReadByte();

            var length = ServerResponsePocketEdition.ServerResponse[packetId]().Size;
            var data = _stream.ReadByteArray(length - 1);

            OnDataReceived(packetId, data);

            _baseSock.EndReceive(ar);
            _baseSock.BeginReceive(new byte[0], 0, 0, SocketFlags.None,  PacketReceiverPocketEditionAsync, _baseSock);
        }
        
        /// <summary>
        /// Packets are handled here.
        /// </summary>
        /// <param name="id">Packet ID</param>
        /// <param name="data">Packet byte[] data</param>
        private void HandlePacketPocketEdition(int id, byte[] data)
        {
            var reader = new MinecraftDataReader(data, NetworkMode);

            if (ServerResponsePocketEdition.ServerResponse[id] == null)
                return;

            var packet = ServerResponsePocketEdition.ServerResponse[id]().ReadPacket(reader);

            RaisePacketHandled(id, packet, null);
        }
    }
}
