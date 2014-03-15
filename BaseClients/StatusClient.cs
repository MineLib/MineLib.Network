using System;
using MineLib.Network.Enums;
using MineLib.Network.Packets;
using MineLib.Network.Packets.Client.Status;
using MineLib.Network.Packets.Server.Status;
using Newtonsoft.Json;

namespace MineLib.Network.BaseClients
{
    public struct Sample
    {
        [JsonProperty("name")]
        public string Name;

        [JsonProperty("id")]
        public string ID;
    }

    public struct Players
    {
        [JsonProperty("max")]
        public int Max;

        [JsonProperty("online")]
        public int Online;

        [JsonProperty("sample")]
        public Sample[] Sample;
    }

    public struct ServerVersion
    {
        [JsonProperty("name")]
        public string Name;

        [JsonProperty("protocol")]
        public int Protocol;
    }

    public struct ServerInfo
    {
        [JsonProperty("description")]
        public string Description;

        [JsonProperty("players")]
        public Players Players;

        [JsonProperty("version")]
        public ServerVersion Version;

        [JsonProperty("favicon")]
        public string Favicon;
    }

    public struct ResponseData
    {
        public ServerInfo Info;
        public int Ping;
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

            int ping = 0;
            int startTime = 0;

            var Info = new ServerInfo();

            FireResponsePacket += packet => Info = ParseResponse(packet);

            FirePingPacket += packet =>
            {
                var Ping = (Packets.Server.Status.PingPacket)packet;
                int currtime = DateTime.UtcNow.Millisecond;
                ping = currtime - startTime;
                Ready = true;
            };

            PingServer(protocolVersion, out startTime);

            while (!Ready) {}
            Disconnect();
            return new ResponseData {Info = Info, Ping = ping};
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

            while (!Ready) {}
            Disconnect();
            return Info;
        }

        public int GetPing(int protocolVersion)
        {
            if (_connectionClosed) throw new Exception("Initialize new StatusClient");

            bool Ready = false;

            int ping = 0;
            int startTime = 0;

            FirePingPacket += packet =>
            {
                var Ping = (Packets.Server.Status.PingPacket)packet;
                int currtime = DateTime.UtcNow.Millisecond;
                ping = currtime - startTime;
                Ready = true;
            };

            PingServer(protocolVersion, out startTime);

            while (!Ready) {}
            Disconnect();
            return ping;
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

        private void PingServer(int protocolVersion, out int ping)
        {
            SendPacket(new HandshakePacket
            {
                ServerAddress = ServerIP,
                ServerPort = ServerPort,
                ProtocolVersion = protocolVersion,
                NextState = NextState.Status
            });

            SendPacket(new RequestPacket());

            ping = DateTime.UtcNow.Millisecond;

            SendPacket(new Packets.Client.Status.PingPacket {Time = ping});
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