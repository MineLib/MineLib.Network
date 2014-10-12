using MineLib.Network.Events;

namespace MineLib.Network.Main.BaseClients
{
    public partial class ServerInfoParser
    {
        public event PacketHandler FirePingPacket;
        public event PacketHandler FireResponsePacket;
    }
}