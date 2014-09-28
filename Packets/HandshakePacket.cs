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
        public byte ID { get { return PacketID; } }

        public void ReadPacket(PacketByteReader reader)
        {
            ProtocolVersion = reader.ReadVarInt();
            ServerAddress = reader.ReadString();
            ServerPort = reader.ReadShort();
            NextState = (NextState) reader.ReadVarInt();
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(ID);
            stream.WriteVarInt(ProtocolVersion);
            stream.WriteString(ServerAddress);
            stream.WriteShort(ServerPort);
            stream.WriteVarInt((byte) NextState);
            stream.Purge();
        }
    }

}