using MineLib.Network.Enums;
using MineLib.Network.Events;
using MineLib.Network.Packets;

namespace MineLib.Network
{
    public partial class NetworkHandler
    {
        public event PacketsHandler OnPacketHandled;

        private void RaisePacketHandled(IPacket packet, int id, ServerState state)
        {
            if (OnPacketHandled != null)
                OnPacketHandled(packet, id, state);
        }
    }
}