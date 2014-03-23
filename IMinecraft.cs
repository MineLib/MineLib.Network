using MineLib.Network.Enums;

namespace MineLib.Network
{
    public interface IMinecraft
    {
        string ServerIP { get; set; }
        short ServerPort { get; set; }

        ServerState State { get; set; }

        string AccessToken { get; set; }
        string SelectedProfile { get; set; }
    }
}