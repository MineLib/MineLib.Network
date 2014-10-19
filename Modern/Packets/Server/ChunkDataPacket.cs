using MineLib.Network.IO;
using MineLib.Network.Modern.Data.Anvil;

namespace MineLib.Network.Modern.Packets.Server
{
    public struct ChunkDataPacket : IPacket
    {
        public Chunk Chunk;

        public byte ID { get { return 0x21; } }

        public IPacket ReadPacket(MinecraftDataReader reader)
        {
            Chunk = Chunk.FromReader(reader);

            return this;
        }

        public IPacket WritePacket(MinecraftStream stream)
        {
            stream.WriteVarInt(ID);
            Chunk.ToStream(stream);
            stream.Purge();

            return this;
        }
    }
}