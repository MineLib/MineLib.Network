using MineLib.Network.IO;
using MineLib.Network.Main.Data.Anvil;

namespace MineLib.Network.Main.Packets.Server
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