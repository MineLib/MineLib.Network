using System;
using System.Collections.Generic;
using System.Net.Sockets;
using MineLib.Network.IO;

namespace MineLib.Network
{
    public sealed partial class NetworkHandler : IDisposable
    {
        // -- Debugging
        public readonly List<IPacket> PacketsReceived = new List<IPacket>();
        public readonly List<IPacket> PacketsSended = new List<IPacket>();
        // -- Debugging.

        #region Properties

        public NetworkMode NetworkMode { get; private set; }

        public bool DebugPackets { get; set; }

        public bool Connected { get { return _baseSock.Connected; } }

        public bool Crashed { get; private set; }

        #endregion

        private readonly IMinecraftClient _minecraft;

        private Socket _baseSock;
        private MinecraftStream _stream;

        private bool _disposed;

        public NetworkHandler(IMinecraftClient client, NetworkMode mode)
        {
            _minecraft = client;
            NetworkMode = mode;
            Crashed = false;
        }

        /// <summary>
        /// Starts the network handler.
        /// </summary>
        public void Start(bool debugPackets = true)
        {
            DebugPackets = debugPackets;

            // -- Connect to server.
            try
            {
                switch (NetworkMode)
                {
                    case NetworkMode.Modern:
                    case NetworkMode.Classic:
                        _baseSock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                        break;

                    case NetworkMode.PocketEdition:
                        _baseSock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Udp);
                        break;
                }

                _baseSock.Connect(_minecraft.ServerHost, _minecraft.ServerPort);
            }
            catch (SocketException)
            {
                Crashed = true;
                return;
            }

            // -- Create our Wrapped socket.
            _stream = new MinecraftStream(new NetworkStream(_baseSock), NetworkMode);

            // -- Subscribe to DataReceived event.
            switch (NetworkMode)
            {
                case NetworkMode.Modern:
                    OnDataReceived += HandlePacketModern;
                    _stream.BeginRead(new byte[0], 0, 0, PacketReceiverModernAsync, null);
                    break;

                case NetworkMode.Classic:
                    OnDataReceived += HandlePacketClassic;
                    _stream.BeginRead(new byte[0], 0, 0, PacketReceiverClassicAsync, null);
                    break;

                case NetworkMode.PocketEdition:
                    OnDataReceived += HandlePacketPocketEdition;
                    _stream.BeginRead(new byte[0], 0, 0, PacketReceiverPocketEditionAsync, null);
                    break;
            }
        }

        public void Send(IPacket packet)
        {
            if (DebugPackets)
                PacketsSended.Add(packet);

            _stream.BeginWrite(packet, null, null);
        }

        /// <summary>
        /// Stops and dispose the network handler.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                if (_baseSock != null)
                    _baseSock.Close();

                if (_stream != null)
                    _stream.Dispose();
            }

            _disposed = true;
        }

        ~NetworkHandler()
        {
            Dispose(false);
        }
    }
}