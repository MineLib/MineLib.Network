using System;
using System.Drawing;
using System.IO;
using MineLib.Network.Data;
using MineLib.Network.Enums;
using MineLib.Network.Packets;
using MineLib.Network.Packets.Client.Status;
using MineLib.Network.Packets.Server.Status;

namespace MineLib.Network.BaseClients
{
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

            FireResponsePacket += packet => ParseResponse(packet, ref Info);

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
                ParseResponse(packet, ref Info);
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

        private void ParseResponse(IPacket packet, ref ServerInfo info)
        {
            // Very dirty, yep.

            var response = (ResponsePacket)packet;
            string Text = response.Response;

            string description = "";
            string max = "";
            string online = "";
            string name = "";
            string protocol = "";
            string favicon = "";
            Image FIcon = null;

            string[] temp = Text.Split(new[] {"description\":\""}, StringSplitOptions.RemoveEmptyEntries);
            if (temp.Length >= 2)
                description = temp[1].Split('"')[0];
            

            temp = Text.Split(new[] {"max\":"}, StringSplitOptions.RemoveEmptyEntries);
            if (temp.Length >= 2)
                max = temp[1].Split(',')[0].Split('}')[0];
            

            temp = Text.Split(new[] {"online\":"}, StringSplitOptions.RemoveEmptyEntries);
            if (temp.Length >= 2)
                online = temp[1].Split(',')[0].Split('}')[0];
            

            temp = Text.Split(new[] {"name\":\""}, StringSplitOptions.RemoveEmptyEntries);
            if (temp.Length >= 2)
                name = temp[1].Split('"')[0];
            

            temp = Text.Split(new[] {"protocol\":"}, StringSplitOptions.RemoveEmptyEntries);
            if (temp.Length >= 2)
                protocol = temp[1].Split('}')[0];
            

            temp = Text.Split(new[] {"favicon\":"}, StringSplitOptions.RemoveEmptyEntries);
            if (temp.Length >= 2)
                favicon = temp[1].Split(',')[1].Replace("\\u003d", "=").Split('\"')[0];
            

            // Big problems with base64 decoding.
            try
            {
                byte[] data = Convert.FromBase64String(favicon);

                using (var ms = new MemoryStream(data))
                {
                    FIcon = Image.FromStream(ms);
                }
            }
            catch (Exception) {}

            info = new ServerInfo
            {
                Description = description,
                Players = new Players {Max = Int32.Parse(max), Online = Int32.Parse(online)},
                Version = new ServerVersion {Name = name, Protocol = Int32.Parse(protocol)},
                Favicon = FIcon
            };
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