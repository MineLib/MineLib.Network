using System;
using System.Collections.Generic;
using MineLib.Network.Events;

namespace MineLib.Network
{
    public interface INetworkHandler : IDisposable
    {
        event PacketHandler OnPacketHandled;

        // -- Debugging
        List<IPacket> PacketsReceived { get; set;}
        List<IPacket> PacketsSended { get; set; }
        // -- Debugging

        // -- Modern
        bool CompressionEnabled { get; }
        long CompressionThreshold { get; }
        // -- Modern

        NetworkMode NetworkMode { get; }

        bool DebugPackets { get; set; }

        bool Connected { get; }
        bool Crashed { get; }


        void Start(bool debugPackets = true);

        IAsyncResult BeginSendPacket(IPacket packet, AsyncCallback asyncCallback, object state);
        IAsyncResult BeginSend(IPacket packet, AsyncCallback asyncCallback, object state);
        void EndSend(IAsyncResult asyncResult);
    }
}