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

        public NetworkMode NetworkMode { get { return _minecraft.Mode; } }

        public bool DebugPackets { get; set; }

        public bool Connected { get { return _baseSock.Connected; } }

        public bool Crashed { get; private set; }

        #endregion

        private readonly IMinecraftClient _minecraft;

        private Socket _baseSock;
        private MinecraftStream _stream;

        public NetworkHandler(IMinecraftClient client)
        {
            _minecraft = client;
        }

        /// <summary>
        /// Start NetworkHandler.
        /// </summary>
        public void Start(bool debugPackets = true)
        {
            DebugPackets = debugPackets;

            // -- Connect to server.
            switch (NetworkMode)
            {
                case NetworkMode.Modern:
                    _baseSock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    _baseSock.BeginConnect(_minecraft.ServerHost, _minecraft.ServerPort, ConnectedModern, null);
                    break;

                case NetworkMode.Classic:
                    _baseSock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    _baseSock.BeginConnect(_minecraft.ServerHost, _minecraft.ServerPort, ConnectedClassic, null);
                    break;

                case NetworkMode.PocketEdition:
                    _baseSock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Udp);
                    _baseSock.BeginConnect(_minecraft.ServerHost, _minecraft.ServerPort, ConnectedPocketEdition, null);
                    break;
            }
        }

        public IAsyncResult BeginSendPacket(IPacket packet, AsyncCallback asyncCallback, object state)
        {
            if (!Connected)
                throw new Exception("Connection error");

            if (DebugPackets)
                PacketsSended.Add(packet);

            IAsyncResult result = BeginSend(packet, asyncCallback, state);
            EndSend(result);
            return result;
        }

        public IAsyncResult BeginSend(IPacket packet, AsyncCallback asyncCallback, object state)
        {
            return _stream.BeginWrite(packet, asyncCallback, state);
        }

        public void EndSend(IAsyncResult asyncResult)
        {
            _stream.EndWrite(asyncResult);
        }

        /// <summary>
        /// Dispose NetworkHandler.
        /// </summary>
        public void Dispose()
        {
            if (_baseSock != null)
                _baseSock.Close();
            
            if (_stream != null)        
                _stream.Dispose();       

            if (PacketsReceived != null)
                PacketsReceived.Clear();

            if (PacketsSended != null)
                PacketsSended.Clear();
        }
    }
}