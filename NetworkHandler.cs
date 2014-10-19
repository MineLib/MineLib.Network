using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using MineLib.Network.IO;

namespace MineLib.Network
{
    public sealed partial class NetworkHandler : IDisposable
    {
        // -- Debugging
        public readonly List<IPacket> PacketsReceived = new List<IPacket>();
        public readonly List<IPacket> PacketsSended = new List<IPacket>();
        // -- Debugging.

        private readonly IMinecraftClient _minecraft;

        private Socket _baseSock;
        private MinecraftStream _stream;

        private Thread _readerThread;

        private bool _disposed;

        #region Properties

        public NetworkMode NetworkMode { get; private set; }

        public bool DebugPackets { get; set; }

        public bool Connected
        {
            get { return _baseSock.Connected; }
        }

        public bool Crashed { get; private set; }

        #endregion

        public NetworkHandler(IMinecraftClient client, NetworkMode mode)
        {
            _minecraft = client;
            NetworkMode = mode;
            Crashed = false;
        }

        /// <summary>
        /// Starts the network handler.
        /// </summary>
        public void Start(bool sync = true, bool debugPackets = true)
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
            var nStream = new NetworkStream(_baseSock);
            var bStream = new BufferedStream(nStream);
            _stream = new MinecraftStream(bStream, NetworkMode);

            // -- Subscribe to DataReceived event.
            switch (NetworkMode)
            {
                    // -- We can choose if we want to handle any packet
                case NetworkMode.Modern:
                    OnDataReceived += HandlePacketModern;
                    break;

                    // -- In Classic and PocketEdition we need to handle every packet
                case NetworkMode.Classic:
                    OnDataReceived += HandlePacketClassic;
                    break;

                case NetworkMode.PocketEdition:
                    OnDataReceived += HandlePacketPocketEdition;
                    break;
            }

            if (sync)
            {
                switch (NetworkMode)
                {
                    case NetworkMode.Modern:
                        _readerThread = new Thread(StartReceivingModernSync) {Name = "PacketListener"};
                        break;

                    case NetworkMode.Classic:
                        _readerThread = new Thread(StartReceivingClassicSync) {Name = "PacketListenerClassic"};
                        break;

                    case NetworkMode.PocketEdition:
                        _readerThread = new Thread(StartReceivingPocketEditionSync)
                        {
                            Name = "PacketListenerPocketEdition"
                        };
                        break;
                }
                _readerThread.Start();
            }
            else // -- Async
            {
                switch (NetworkMode)
                {
                    case NetworkMode.Modern:
                        _baseSock.BeginReceive(new byte[0], 0, 0, SocketFlags.None, PacketReceiverModernAsync, _baseSock);
                        break;

                    case NetworkMode.Classic:
                        _baseSock.BeginReceive(new byte[0], 0, 0, SocketFlags.None, PacketReceiverClassicAsync,
                            _baseSock);
                        break;

                    case NetworkMode.PocketEdition:
                        _baseSock.BeginReceive(new byte[0], 0, 0, SocketFlags.None, PacketReceiverPocketEditionAsync,
                            _baseSock);
                        break;
                }
            }
        }

        public void Send(IPacket packet)
        {
            if (DebugPackets)
                PacketsSended.Add(packet);

            using (var ms = new MemoryStream())
            {
                packet.WritePacket(new MinecraftStream(ms, NetworkMode));
                var data = ms.ToArray();

                _baseSock.BeginSend(data, 0, data.Length, SocketFlags.None, null, null);
            }
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

                if (_readerThread != null && _readerThread.IsAlive)
                    _readerThread.Abort();
            }

            _disposed = true;
        }

        ~NetworkHandler()
        {
            Dispose(false);
        }
    }
}