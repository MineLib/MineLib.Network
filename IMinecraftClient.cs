using MineLib.Network.Enums;

namespace MineLib.Network
{
    public interface IMinecraftClient
    {
        string ServerHost { get; set; }
        short ServerPort { get; set; }

        ServerState State { get; set; }

        string AccessToken { get; set; }
        string SelectedProfile { get; set; }

        void Dispose();
    }
}