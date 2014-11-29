using MineLib.Network.Classic.Enums;
using MineLib.Network.Data;
using MineLib.Network.IO;

namespace MineLib.Network.Classic.Packets.Client
{
    public struct SetBlockPacket : IPacketWithSize
    {
        public Position Coordinates;
        public SetBlockMode Mode;
        public byte BlockType;

        public byte ID { get { return 0x05; } }
        public short Size { get { return 9; } }

        public IPacketWithSize ReadPacket(IMinecraftDataReader reader)
        {
            Coordinates = Position.FromReaderShort(reader);
            Mode = (SetBlockMode) reader.ReadByte();
            BlockType = reader.ReadByte();

            return this;
        }

        IPacket IPacket.ReadPacket(IMinecraftDataReader reader)
        {
            return ReadPacket(reader);
        }

        public IPacket WritePacket(IMinecraftStream stream)
        {
            stream.WriteByte(ID);
            Coordinates.ToStreamShort(stream);
            stream.WriteByte((byte) Mode);
            stream.WriteByte(BlockType);
            stream.Purge();

            return this;
        }
    }
}
