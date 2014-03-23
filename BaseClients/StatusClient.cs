using System;
using System.Diagnostics;
using MineLib.Network.Enums;
using MineLib.Network.Packets;
using MineLib.Network.Packets.Client.Status;
using MineLib.Network.Packets.Server.Status;
using Newtonsoft.Json;

namespace MineLib.Network.BaseClients
{
    public struct ResponseData
    {
        public ServerInfo Info;
        public long Ping;
    }

    public partial class StatusClient : IMinecraft, IDisposable
    {
        #region Variables

        public string ServerIP { get; set; }

        public short ServerPort { get; set; }

        public ServerState State { get; set; }

        public string AccessToken { get; set; }

        public string SelectedProfile { get; set; }

        #endregion Variables

        private readonly NetworkHandler _handler;
        private bool _connectionClosed;

        public StatusClient(string ip, short port)
        {
            State = ServerState.Status;

            ServerIP = ip;
            ServerPort = port;

            _handler = new NetworkHandler(this);

            // -- Register our event handlers.
            _handler.OnPacketHandled += RaisePacketHandled;

            // -- Connect to the server and begin reading packets.
            _handler.Start();
        }

        public ResponseData GetServerInfo(int protocolVersion)
        {
            if (_connectionClosed) throw new Exception("Initialize new StatusClient");

            bool Ready = false;

            var stopwatch = new Stopwatch();

            var Info = new ServerInfo();

            FireResponsePacket += packet => Info = ParseResponse(packet);

            FirePingPacket += packet =>
            {
                stopwatch.Stop();
                Ready = true;
            };

            stopwatch.Start();
            PingServer(protocolVersion);

            int x = 0;
            while (!Ready)
            {
                x++;
                if (x > 200)
                    break;
            }
            Disconnect();
            return new ResponseData {Info = Info, Ping = stopwatch.ElapsedMilliseconds};
        }

        public ServerInfo GetInfo(int protocolVersion)
        {
            if (_connectionClosed) throw new Exception("Initialize new StatusClient");

            bool Ready = false;

            var Info = new ServerInfo();

            FireResponsePacket += packet =>
            {
                Info = ParseResponse(packet);
                Ready = true;
            };

            RequestServerInfo(protocolVersion);

            int x = 0;
            while (!Ready)
            {
                x++;
                if (x > 200)
                    break;
            }
            Disconnect();
            return Info;
        }

        public long GetPing(int protocolVersion)
        {
            if (_connectionClosed) throw new Exception("Initialize new StatusClient");

            bool Ready = false;

            var stopwatch = new Stopwatch();

            FirePingPacket += packet =>
            {
                var Ping = (Packets.Server.Status.PingPacket)packet;
                stopwatch.Stop();
                Ready = true;
            };

            PingServer(protocolVersion);

            while (!Ready) {}
            Disconnect();
            return stopwatch.ElapsedMilliseconds;
        }

        private void RequestServerInfo(int protocolVersion)
        {
            SendPacket(new HandshakePacket
            {
                ServerAddress = ServerIP,
                ServerPort = ServerPort,
                ProtocolVersion = protocolVersion,
                NextState = NextState.Status
            });

            SendPacket(new RequestPacket());
        }

        private void PingServer(int protocolVersion)
        {
            SendPacket(new HandshakePacket
            {
                ServerAddress = ServerIP,
                ServerPort = ServerPort,
                ProtocolVersion = protocolVersion,
                NextState = NextState.Status
            });

            SendPacket(new RequestPacket());

            SendPacket(new Packets.Client.Status.PingPacket { Time = DateTime.UtcNow.Millisecond });
        }

        private ServerInfo ParseResponse(IPacket packet)
        {
            var response = (ResponsePacket)packet;

            return JsonConvert.DeserializeObject<ServerInfo>(response.Response);
        }

        private void SendPacket(IPacket packet)
        {
            if (_handler != null)
                _handler.Send(packet);
        }

        private void Disconnect()
        {
            if (_handler != null)
                _handler.Stop();

            _connectionClosed = true;
        }

        public void Dispose()
        {
            Disconnect();
        }
    }
}