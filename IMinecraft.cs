using MineLib.Network.Enums;

namespace MineLib.Network
{
    public interface IMinecraft
    {
        string ServerIP { get; set; }
        int ServerPort { get; set; }

        bool Running { get; set; }

        ServerState State { get; set; }
    }
}
