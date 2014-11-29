using MineLib.Network.Classic.Enums;
using MineLib.Network.IO;

namespace MineLib.Network.Classic.Packets.Server
{
    public struct ServerIdentificationPacket : IPacketWithSize
    {
        public byte ProtocolVersion;
        public string ServerName;
        public string ServerMOTD;
        public UserType UserType;

        public byte ID { get { return 0x00; } }
        public short Size { get { return 131; } }

        public IPacketWithSize ReadPacket(IMinecraftDataReader reader)
        {
            ProtocolVersion = reader.ReadByte();
            ServerName = reader.ReadString();
            ServerMOTD = reader.ReadString();
            UserType = (UserType) reader.ReadByte();

            return this;
        }

        IPacket IPacket.ReadPacket(IMinecraftDataReader reader)
        {
            return ReadPacket(reader);
        }

        public IPacket WritePacket(IMinecraftStream stream)
        {
            stream.WriteByte(ID);
            stream.WriteByte(ProtocolVersion);
            stream.WriteString(ServerName);
            stream.WriteString(ServerMOTD);
            stream.WriteByte((byte) UserType);
            stream.Purge();

            return this;
        }
    }
}
