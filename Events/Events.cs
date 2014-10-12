namespace MineLib.Network.Events
{
    public delegate void PacketsHandler(int id, IPacket packet, ServerState? state);
    public delegate void PacketHandler(IPacket packet);

}
