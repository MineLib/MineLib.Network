using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using MineLib.Network.Events;
using MineLib.Network.IO;

namespace MineLib.Network
{
    public partial class NetworkHandler : IDisposable
    {
        // -- Debugging
        public readonly List<IPacket> PacketsReceived = new List<IPacket>();
        public readonly List<IPacket> PacketsSended = new List<IPacket>();
        // -- Debugging.

        private delegate void DataReceived(int id, byte[] data);
        private event DataReceived OnDataReceived;
        
        public event PacketsHandler OnPacketHandled;

        private Socket _baseSock;
        private PacketStream _stream;
        private PacketByteReader _reader;

        private Thread _listener;

        private readonly IMinecraftClient _minecraft;

        public NetworkMode Mode { get; private set; }

        public bool DebugPackets { get; set; }

        public bool Connected { get { return _baseSock.Connected; }}

        public bool Crashed { get; private set; }

        public NetworkHandler(IMinecraftClient client, NetworkMode mode = NetworkMode.Main)
        {
            _minecraft = client;
            Mode = mode;
            Crashed = false;
        }

        /// <summary>
        ///     Starts the network handler.
        /// </summary>
        public void Start(bool sync = false, bool debugPackets = true)
        {
            DebugPackets = debugPackets;

            // -- Connect to server.
            try
            {
                switch (Mode)
                {
                    case NetworkMode.Main:
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
            _stream = new PacketStream(bStream);

            // -- Subscribe to DataReceived event.
            switch (Mode)
            {
                // -- We can choose if we want to handle any packet
                case NetworkMode.Main:
                    OnDataReceived += HandlePacketMain;
                    break;

                // -- In Classic and PocketEdition we need to handle every packet
                case NetworkMode.Classic:
                    OnDataReceived += HandlePacketClassic;
                    //OnPacketHandledClassic += RaisePacketHandledClassic;
                    break;

                case NetworkMode.PocketEdition:
                    OnDataReceived += HandlePacketPocketEdition;
                    break;
            }

            if (sync)
            {
                switch (Mode)
                {
                    case NetworkMode.Main:
                        _listener = new Thread(StartReceivingMainSync) {Name = "PacketListener"};
                        break;

                    case NetworkMode.Classic:
                        _listener = new Thread(StartReceivingClassicSync) {Name = "PacketListenerClassic"};
                        break;

                    case NetworkMode.PocketEdition:
                        _listener = new Thread(StartReceivingPocketEditionSync) { Name = "PacketListenerPocketEdition" };
                        break;
                }
                _listener.Start();
            }
            else
            {
                switch (Mode)
                {
                    case NetworkMode.Main:
                        _baseSock.BeginReceive(new byte[0], 0, 0, SocketFlags.None, PacketReceiverMainAsync, _baseSock);
                        break;

                    case NetworkMode.Classic:
                        _baseSock.BeginReceive(new byte[0], 0, 0, SocketFlags.None, PacketReceiverClassicAsync, _baseSock);
                        break;

                    case NetworkMode.PocketEdition:
                        _baseSock.BeginReceive(new byte[0], 0, 0, SocketFlags.None, PacketReceiverPocketEditionAsync, _baseSock);
                        break;
                }
            }
        }

        public void Send(IPacket packet)
        {
            if (DebugPackets)
                PacketsSended.Add(packet);

            var ms = new MemoryStream();
            var stream = new PacketStream(ms, Mode);
            packet.WritePacket(ref stream);
            var data = ms.ToArray();

            _baseSock.BeginSend(ms.ToArray(), 0, data.Length, SocketFlags.None, null, null);
        }

        /// <summary>
        ///     Stops and dispose the network handler.
        /// </summary>
        public void Dispose()
        {
            if (_listener != null && _listener.IsAlive)
                _listener.Abort();

            if (_baseSock != null)
                _baseSock.Close();

            if (_stream != null)
                _stream.Dispose();

            if (_reader != null)
                _reader.Dispose();
        }
    }
}