using MineLib.Network.Events;
using MineLib.Network.Packets;

namespace MineLib.Network
{
    public partial class NetworkHandler
    {
        public event PacketsHandler OnPacketHandledClassic;

        private void RaisePacketHandledClassic(IPacket packet, int id)
        {
            if (OnPacketHandledClassic != null)
                OnPacketHandledClassic(packet, id, null);
        }
    }
}
