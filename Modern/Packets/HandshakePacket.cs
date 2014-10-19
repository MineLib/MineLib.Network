using MineLib.Network.IO;
using MineLib.Network.Modern.Enums;

namespace MineLib.Network.Modern.Packets
{
    public struct HandshakePacket : IPacket
    {
        public int ProtocolVersion;
        public string ServerAddress;
        public short ServerPort;
        public NextState NextState;

        public const byte PacketID = 0x00;
        public byte ID { get { return PacketID; } }

        public IPacket ReadPacket(MinecraftDataReader reader)
        {
            ProtocolVersion = reader.ReadVarInt();
            ServerAddress = reader.ReadString();
            ServerPort = reader.ReadShort();
            NextState = (NextState) reader.ReadVarInt();

            return this;
        }

        public IPacket WritePacket(MinecraftStream stream)
        {
            stream.WriteVarInt(ID);
            stream.WriteVarInt(ProtocolVersion);
            stream.WriteString(ServerAddress);
            stream.WriteShort(ServerPort);
            stream.WriteVarInt((byte) NextState);
            stream.Purge();

            return this;
        }
    }
}