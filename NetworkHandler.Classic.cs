﻿using MineLib.Network.Classic.Packets;
using MineLib.Network.IO;

namespace MineLib.Network
{
    public partial class NetworkHandler
    {
        #region Sending and Receiving.

        private void StartReceivingClassic()
        {
            do
            {
            } while (PacketReceiverClassic());
        }

        private bool PacketReceiverClassic()
        {
            if (_baseSock.Client == null || !Connected)
                return false;

            while (_baseSock.Client.Available > 0)
            {
                var length = _stream.ReadVarInt();
                var id = _stream.ReadByte();

                OnDataReceivedClassic(id, _stream.ReadByteArray(length - 1));
            }

            return true;
        }

        #endregion Sending and Receiving.

        private void HandlePacketClassic(int id, byte[] data)
        {
            _reader = new PacketByteReader(data);

            if (ServerResponseClassic.ServerResponse[id] == null)
                return;

            var packet = ServerResponseClassic.ServerResponse[id]();
            packet.ReadPacket(_reader);

#if DEBUG
            PacketsReceived.Add(packet);
#endif

            RaisePacketHandledClassic(packet, id);
        }
    }
}
