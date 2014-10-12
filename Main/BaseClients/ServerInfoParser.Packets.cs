using MineLib.Network.Main.Enums;

namespace MineLib.Network.Main.BaseClients
{
    public partial class ServerInfoParser
    {
        private void RaisePacketHandled(int id, IPacket packet, ServerState? state)
        {
            if (state != ServerState.MainStatus) return;

            switch ((PacketsServer) id)
            {
                case PacketsServer.Ping:
                    if (FirePingPacket != null)
                        FirePingPacket(packet);
                    break;

                case PacketsServer.Response:
                    if (FireResponsePacket != null)
                        FireResponsePacket(packet);
                    break;
            }
        }
    }
}