using MineLib.Network.IO;
using MineLib.Network.Modern.Data;

namespace MineLib.Network.Classic.Packets.Server
{
    public struct SetBlockPacket : IPacketWithSize
    {
        public Position Coordinates;
        public byte BlockType;

        public byte ID { get { return 0x06; } }
        public short Size { get { return 8; } }

        public IPacketWithSize ReadPacket(IMinecraftDataReader stream)
        {
            Coordinates.X = stream.ReadShort();
            Coordinates.Y = stream.ReadShort();
            Coordinates.Z = stream.ReadShort();
            BlockType = stream.ReadByte();

            return this;
        }

        IPacket IPacket.ReadPacket(IMinecraftDataReader stream)
        {
            return ReadPacket(stream);
        }

        public IPacket WritePacket(IMinecraftStream stream)
        {
            stream.WriteByte(ID);
            stream.WriteShort((short)Coordinates.X);
            stream.WriteShort((short)Coordinates.Y);
            stream.WriteShort((short)Coordinates.Z);
            stream.WriteByte(BlockType);
            stream.Purge();

            return this;
        }
    }
}
