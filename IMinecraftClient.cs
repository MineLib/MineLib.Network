using System;

namespace MineLib.Network
{
    public interface IMinecraftClient : IDisposable
    {
        string ServerHost { get; set; }
        short ServerPort { get; set; }

        ServerState State { get; set; }

        // -- Modern
        string AccessToken { get; set; }
        string SelectedProfile { get; set; }
        // -- Modern

        NetworkMode Mode { get; set; }
    }
}