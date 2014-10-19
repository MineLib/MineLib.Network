using MineLib.Network.IO;
using MineLib.Network.Modern.Data;

namespace MineLib.Network.Modern.Packets.Server
{
    public struct BlockChangePacket : IPacket
    {
        public Position Location;
        public int BlockIDMeta;

        public byte ID { get { return 0x23; } }

        public IPacket ReadPacket(MinecraftDataReader reader)
        {
            Location = Position.FromLong(reader.ReadLong());
            BlockIDMeta = reader.ReadVarInt();

            return this;
        }

        public IPacket WritePacket(MinecraftStream stream)
        {
            stream.WriteVarInt(ID);
            Location.ToStreamLong(stream);
            stream.WriteVarInt(BlockIDMeta);
            stream.Purge();

            return this;
        }
    }
}