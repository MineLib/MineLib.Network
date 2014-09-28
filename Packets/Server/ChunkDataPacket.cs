using MineLib.Network.Data.Anvil;
using MineLib.Network.IO;

namespace MineLib.Network.Packets.Server
{
    public struct ChunkDataPacket : IPacket
    {
        public Chunk Chunk;

        public byte ID { get { return 0x21; } }

        public void ReadPacket(PacketByteReader reader)
        {
            Chunk = Chunk.FromReader(reader);
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(ID);
            Chunk.ToStream(ref stream);
            stream.Purge();
        }
    }
}