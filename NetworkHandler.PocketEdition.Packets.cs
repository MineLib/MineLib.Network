namespace MineLib.Network
{
    public partial class NetworkHandler
    {
        private void RaisePacketHandledPocketEdition(int id, IPacket packet, ServerState? state)
        {
            if (DebugPackets)
                PacketsReceived.Add(packet);

            if (OnPacketHandled != null)
                OnPacketHandled(id, packet, null);
        }
    }
}
