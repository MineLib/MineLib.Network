using MineLib.Network.Events;

namespace MineLib.Network.Main.BaseClients
{
    public partial class ServerInfoParser
    {
        public event PacketsHandler FirePingPacket;
        public event PacketsHandler FireResponsePacket;
    }
}