namespace MineLib.Network.Events
{
    public delegate void PacketsHandler(IPacket packet);
    public delegate void PacketHandler(int id, IPacket packet, ServerState? state);
    public delegate void DataReceived(int id, byte[] data);
}
