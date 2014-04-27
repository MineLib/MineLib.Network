using MineLib.Network.IO;
using MineLib.Network.Packets;

namespace MineLib.Network.Classic.Packets.Server
{
    public struct ServerIdentificationPacket : IPacket
    {
        public byte ProtocolVersion;
        public string ServerName;
        public string ServerMOTD;
        public byte UserType;

        public const byte PacketID = 0x00;
        public byte Id { get { return PacketID; } }

        public void ReadPacket(PacketByteReader stream)
        {
            ProtocolVersion = stream.ReadByte();
            ServerName = stream.ReadString();
            ServerMOTD = stream.ReadString();
            UserType = stream.ReadByte();
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteByte(Id);
            stream.WriteByte(ProtocolVersion);
            stream.WriteString(ServerName);
            stream.WriteString(ServerMOTD);
            stream.WriteByte(UserType);
            stream.Purge();
        }
    }
}
