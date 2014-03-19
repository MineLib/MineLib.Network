using MineLib.Network.IO;
using MineLib.Network.Enums;


namespace MineLib.Network.Packets
{
    public struct HandshakePacket : IPacket
    {
        public int ProtocolVersion;
        public string ServerAddress;
        public short ServerPort;
        public NextState NextState;

        public const byte PacketID = 0x00;
        public byte Id { get { return PacketID; } }

        public void ReadPacket(PacketByteReader stream)
        {
            ProtocolVersion = stream.ReadVarInt();
            ServerAddress = stream.ReadString();
            ServerPort = stream.ReadShort();
            NextState = (NextState)stream.ReadVarInt();
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(Id);
            stream.WriteVarInt(ProtocolVersion);
            stream.WriteString(ServerAddress);
            stream.WriteShort(ServerPort);
            stream.WriteVarInt((byte)NextState);
            stream.Purge();
        }
    }

}