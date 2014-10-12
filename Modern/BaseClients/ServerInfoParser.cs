using System;
using System.Diagnostics;
using System.Net.Sockets;
using System.Threading;
using MineLib.Network.Modern.Enums;
using MineLib.Network.Modern.Packets;
using MineLib.Network.Modern.Packets.Client.Status;
using MineLib.Network.Modern.Packets.Server.Status;
using Newtonsoft.Json;
using PingPacket = MineLib.Network.Modern.Packets.Client.Status.PingPacket;

namespace MineLib.Network.Modern.BaseClients
{
    public struct ResponseData
    {
        public ServerInfo Info;
        public int Ping;
    }

    // TODO: Handle this mess
    public partial class ServerInfoParser : IMinecraftClient, IDisposable
    {
        #region Variables

        public string ServerHost { get; set; }

        public short ServerPort { get; set; }

        public ServerState State { get; set; }

        public string AccessToken
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string SelectedProfile
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public NetworkMode Mode { get; set; }

        #endregion Variables

        private NetworkHandler _handler;
        private bool ConnectionClosed { get { return !_handler.Connected; }}


        public ServerInfoParser()
        {
            Mode = NetworkMode.Modern;
            State = ServerState.ModernStatus;
        }

        public ResponseData GetResponseData(string host, short port, int protocolVersion)
        {
            ServerHost = host;
            ServerPort = port;

            var pingPacketReceived = false;
            var info = new ServerInfo();

            _handler = new NetworkHandler(this, Mode);
            _handler.OnPacketHandled += RaisePacketHandled;
            _handler.Start();

            #region Handle Early connect error

            if (ConnectionClosed)
            {
                Dispose();
                return new ResponseData { Info = new ServerInfo(), Ping = int.MaxValue };
            }

            #endregion

            #region Packet handlers

            FireResponsePacket += packet =>
            {
                // Send Ping Packet after receiving Response Packet
                SendPacket(new PingPacket { Time = DateTime.UtcNow.Millisecond });

                info = ParseResponse(packet);
            };

            FirePingPacket += packet =>
            {
                pingPacketReceived = true;
            };

            #endregion

            RequestServerInfo(protocolVersion);

            #region Waiting for all packets handling and handling if something went wrong

            var watch = Stopwatch.StartNew();
            while (!pingPacketReceived)
            {
                if (_handler == null || _handler.Crashed)
                {
                    watch.Stop();
                    Dispose();
                    return new ResponseData { Info = new ServerInfo(), Ping = int.MaxValue };
                }

                if (watch.ElapsedMilliseconds > 2000)
                {
                    watch.Stop();
                    Dispose();
                    return new ResponseData { Info = new ServerInfo { Version = new Version { Name = "Unsupported", Protocol = 0} }, Ping = (int)PingServer(host, port) };
                }

                Thread.Sleep(50);
            }

            watch.Stop();

            #endregion

            // Create a new TCP connection, if something is wrong use _handler TCP client
            var ping = (int) PingServer(host, port);

            Dispose();
            return new ResponseData { Info = info, Ping = ping };
        }

        public ServerInfo GetServerInfo(string host, short port, int protocolVersion)
        {
            ServerHost = host;
            ServerPort = port;

            var responsePacketReceived = false;
            var info = new ServerInfo();

            _handler = new NetworkHandler(this, Mode);
            _handler.OnPacketHandled += RaisePacketHandled;
            _handler.Start();

            #region Handle Early connect error

            if (ConnectionClosed)
            {
                Dispose();
                return new ServerInfo();
            }

            #endregion
            
            #region Packet handlers

            FireResponsePacket += delegate(IPacket packet)
            {
                info = ParseResponse(packet);
                responsePacketReceived = true;
            };

            #endregion

            RequestServerInfo(protocolVersion);

            #region Waiting for all packets handling and handling if something went wrong

            var watch = Stopwatch.StartNew();
            while (!responsePacketReceived)
            {
                if (_handler != null && _handler.Crashed)
                {
                    Dispose();
                    return new ServerInfo();
                }

                if (watch.ElapsedMilliseconds > 2000)
                {
                    watch.Stop();
                    Dispose();
                    return new ServerInfo { Version = new Version { Name = "Unsupported", Protocol = 0 } };
                }

                Thread.Sleep(50);
            }

            watch.Stop();

            #endregion

            Dispose();
            return info;
        }

        public long GetPing(string host, short port, int protocolVersion)
        {
            ServerHost = host;
            ServerPort = port;

            var pingPacketReceived = false;

            _handler = new NetworkHandler(this, Mode);
            _handler.OnPacketHandled += RaisePacketHandled;
            _handler.Start();

            #region Handle Early connect error

            if (ConnectionClosed)
            {
                Dispose();
                return int.MaxValue;
            }

            #endregion

            #region Packet handlers

            FireResponsePacket +=
                packet => SendPacket(new PingPacket {Time = DateTime.UtcNow.Millisecond});

            FirePingPacket += packet =>
            {
                pingPacketReceived = true;
            };

            #endregion

            RequestServerInfo(protocolVersion);

            #region Waiting for all packets handling and handling if something went wrong

            while (!pingPacketReceived)
            {
                if (_handler != null && ConnectionClosed)
                {
                    Dispose();
                    return int.MaxValue;
                }

                Thread.Sleep(50);
            }

            #endregion

            var ping = PingServer(host, port);

            Dispose();
            return ping;
        }


        private void RequestServerInfo(int protocolVersion)
        {
            SendPacket(new HandshakePacket
            {
                ServerAddress = ServerHost,
                ServerPort = ServerPort,
                ProtocolVersion = protocolVersion,
                NextState = NextState.Status
            });

            SendPacket(new RequestPacket());
        }

        private static ServerInfo ParseResponse(IPacket packet)
        {
            var response = (ResponsePacket)packet;

            return JsonConvert.DeserializeObject<ServerInfo>(response.Response, new Base64Converter());
        }

        private void SendPacket(IPacket packet)
        {
            if (_handler != null)
                _handler.Send(packet);
        }

        private static long PingServer(string host, int port)
        {
            var watch = new Stopwatch();
            watch.Start();
            var client = new TcpClient(host, port);
            watch.Stop();
            client.Close();

            return watch.ElapsedMilliseconds;
        }


        public void Dispose()
        {
            if(_handler != null)
                _handler.Dispose();
        }
    }
}