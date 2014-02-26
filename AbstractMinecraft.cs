using MineLib.Network.Enums;

namespace MineLib.Network
{
    public abstract class Minecraft
    {
        public string ServerIP;
        public int ServerPort;

        public bool Running;

        public ServerState ServerState;
    }
}
