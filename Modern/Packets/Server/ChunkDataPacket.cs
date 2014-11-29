using MineLib.Network.Data.Anvil;
using MineLib.Network.IO;

namespace MineLib.Network.Modern.Packets.Server
{
    public struct ChunkDataPacket : IPacket
    {
        public Chunk Chunk;

        public byte ID { get { return 0x21; } }

        public IPacket ReadPacket(IMinecraftDataReader reader)
        {
            Chunk = Chunk.FromReader(reader);

            return this;
        }

        public IPacket WritePacket(IMinecraftStream stream)
        {
            stream.WriteVarInt(ID);
            Chunk.ToStream(stream);
            stream.Purge();

            return this;
        }
    }
}