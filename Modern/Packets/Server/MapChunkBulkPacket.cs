using MineLib.Network.IO;
using MineLib.Network.Modern.Data.Anvil;

namespace MineLib.Network.Modern.Packets.Server
{
    public struct MapChunkBulkPacket : IPacket
    {
        public ChunkList ChunkList;

        public byte ID { get { return 0x26; } }

        public IPacket ReadPacket(IMinecraftDataReader reader)
        {
            ChunkList = ChunkList.FromReader(reader);

            return this;
        }

        public IPacket WritePacket(IMinecraftStream stream)
        {
            stream.WriteVarInt(ID);
            ChunkList.ToStream(stream);
            stream.Purge();

            return this;
        }
    }
}