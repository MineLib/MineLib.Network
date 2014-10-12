using MineLib.Network.IO;
using MineLib.Network.Main.Data.Anvil;

namespace MineLib.Network.Main.Packets.Server
{
    public struct MapChunkBulkPacket : IPacket
    {
        public ChunkList ChunkList;

        public byte ID { get { return 0x26; } }

        public void ReadPacket(PacketByteReader reader)
        {
            ChunkList = ChunkList.FromReader(reader);
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(ID);
            ChunkList.ToStream(ref stream);
            stream.Purge();
        }
    }
}