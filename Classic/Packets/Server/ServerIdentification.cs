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

        public void ReadPacket(PacketByteReader stream)
        {
            ProtocolVersion = stream.ReadByte();
            ServerName = stream.ReadString();
            ServerMOTD = stream.ReadString();
            UserType = (UserType) stream.ReadByte();
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteByte(ID);
            stream.WriteByte(ProtocolVersion);
            stream.WriteString(ServerName);
            stream.WriteString(ServerMOTD);
            stream.WriteByte((byte) UserType);
            stream.Purge();
        }
    }
}
