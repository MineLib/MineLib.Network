using System;
using System.Net.Sockets;
using System.Threading;
using MineLib.Network.Classic.Packets;
using MineLib.Network.IO;

namespace MineLib.Network
{
    public partial class NetworkHandler
    {
        private void StartReceivingClassicSync()
        {
            do
            {
                Thread.Sleep(50);
            } while (PacketReceiverClassicSync());
        }

        private bool PacketReceiverClassicSync()
        {
            if (_baseSock == null || !Connected)
                return false;

            while (_baseSock.Available > 0)
            {
                var packetId = _stream.ReadByte();

                // Connection lost
                if (packetId == 255)
                {
                    Crashed = true;
                    break;
                }

                var length = ServerResponseClassic.ServerResponse[packetId]().Size;
                var data = _stream.ReadByteArray(length - 1);

                OnDataReceived(packetId, data);
            }

            return true;
        }
        
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
            _baseSock.BeginReceive(new byte[0], 0, 0, SocketFlags.None, PacketReceiverClassicAsync, _baseSock);
        }


        /// <summary>
        /// Packets are handled here.
        /// </summary>
        /// <param name="id">Packet ID</param>
        /// <param name="data">Packet byte[] data</param>
        private void HandlePacketClassic(int id, byte[] data)
        {
            _reader = new PacketByteReader(data, Mode);

            if (ServerResponseClassic.ServerResponse[id] == null)
                return;

            var packet = ServerResponseClassic.ServerResponse[id]();
            packet.ReadPacket(_reader);

            RaisePacketHandledClassic(id, packet, null);
        }
    }
}
