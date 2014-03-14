using MineLib.Network.Events;

namespace MineLib.Network.BaseClients
{
    public partial class StatusClient
    {
        public event PacketHandler FirePingPacket;
        public event PacketHandler FireResponsePacket;
    }
}