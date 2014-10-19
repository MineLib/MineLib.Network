using MineLib.Network.Classic.Enums;
using MineLib.Network.IO;
using MineLib.Network.Modern.Data;

namespace MineLib.Network.Classic.Packets.Client
{
    public struct SetBlockPacket : IPacketWithSize
    {
        public Position Coordinates;
        public SetBlockMode Mode;
        public byte BlockType;

        public byte ID { get { return 0x05; } }
        public short Size { get { return 9; } }

        public IPacketWithSize ReadPacket(MinecraftDataReader stream)
        {
            Coordinates.X = stream.ReadShort();
            Coordinates.Y = stream.ReadShort();
            Coordinates.Z = stream.ReadShort();
            Mode = (SetBlockMode) stream.ReadByte();
            BlockType = stream.ReadByte();

            return this;
        }

        IPacket IPacket.ReadPacket(MinecraftDataReader stream)
        {
            return ReadPacket(stream);
        }

        public IPacket WritePacket(MinecraftStream stream)
        {
            stream.WriteByte(ID);
            stream.WriteShort((short)Coordinates.X);
            stream.WriteShort((short)Coordinates.Y);
            stream.WriteShort((short)Coordinates.Z);
            stream.WriteByte((byte) Mode);
            stream.WriteByte(BlockType);
            stream.Purge();

            return this;
        }
    }
}
