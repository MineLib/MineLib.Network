using MineLib.Network.IO;
using MineLib.Network.Modern.Data;
using MineLib.Network.Modern.Enums;

namespace MineLib.Network.Modern.Packets.Client
{
    public struct PlayerDiggingPacket : IPacket
    {
        public BlockStatus Status;
        public Position Location;
        public byte Face;

        public byte ID { get { return 0x07; } }

        public IPacket ReadPacket(MinecraftDataReader reader)
        {
            Status = (BlockStatus) reader.ReadByte();
            Location = Position.FromReaderLong(reader);
            Face = reader.ReadByte();

            return this;
        }

        public IPacket WritePacket(MinecraftStream stream)
        {
            stream.WriteVarInt(ID);
            stream.WriteByte((byte) Status);
            Location.ToStreamLong(stream);
            stream.WriteByte(Face);
            stream.Purge();

            return this;
        }
    }
}