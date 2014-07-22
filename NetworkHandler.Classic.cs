using System.IO;
using System.Threading;
using MineLib.Network.Classic.Packets;
using MineLib.Network.IO;

namespace MineLib.Network
{
    public partial class NetworkHandler
    {
        #region Sending and Receiving.

        private void StartReceivingClassic()
        {
            _preader = new PacketByteReader(new MemoryStream(0));

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

        private void StartSendingClassic()
        {
            do
            {
            } while (PacketSenderClassic());
        }

        private bool PacketSenderClassic()
        {
            if (_baseSock.Client == null || !Connected)
                return false;

            if (_packetsToSend.Count == 0)
                return true;

            while (_packetsToSend.Count > 0)
            {
                Thread.Sleep(1); // -- Important to make a little pause.
                var packet = _packetsToSend.Dequeue();

#if DEBUG
                _packetsSended.Add(packet);
#endif

                packet.WritePacket(ref _stream);

            }
            return true;
        }

        #endregion Sending and Receiving.

        private void HandlePacketClassic(int id, byte[] data)
        {
            _preader.SetNewData(data);

            if (ServerResponseClassic.ServerResponse[id] == null)
                return;

            var packet = ServerResponseClassic.ServerResponse[id]();
            packet.ReadPacket(_preader);

#if DEBUG
            _packetsReceived.Add(packet);
#endif

            RaisePacketHandledClassic(packet, id);
        }
    }
}
