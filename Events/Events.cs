using MineLib.Network.Enums;
using MineLib.Network.Packets;

namespace MineLib.Network.Events
{
    public delegate void PacketsHandler(object sender, IPacket packet, int id, ServerState state);
    public delegate void PacketHandler(IPacket packet);

}
