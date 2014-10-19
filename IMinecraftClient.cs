using System;

namespace MineLib.Network
{
    public interface IMinecraftClient : IDisposable
    {
        string ServerHost { get; set; }
        short ServerPort { get; set; }

        ServerState State { get; set; }

        string AccessToken { get; set; }
        string SelectedProfile { get; set; }

        NetworkMode Mode { get; set; }
    }
}