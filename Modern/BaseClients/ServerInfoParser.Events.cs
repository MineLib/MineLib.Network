using MineLib.Network.Events;

namespace MineLib.Network.Modern.BaseClients
{
    public partial class ServerInfoParser
    {
        public event PacketsHandler FirePingPacket;
        public event PacketsHandler FireResponsePacket;
    }
}