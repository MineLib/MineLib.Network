using MineLib.Network.IO;
using MineLib.Network.Modern.Data.Anvil;

namespace MineLib.Network.Modern.Packets.Server
{
    public struct MapChunkBulkPacket : IPacket
    {
        public ChunkList ChunkList;

        public byte ID { get { return 0x26; } }

        public IPacket ReadPacket(MinecraftDataReader reader)
        {
            ChunkList = ChunkList.FromReader(reader);

            return this;
        }

        public IPacket WritePacket(MinecraftStream stream)
        {
            stream.WriteVarInt(ID);
            ChunkList.ToStream(stream);
            stream.Purge();

            return this;
        }
    }
}