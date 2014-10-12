using MineLib.Network.IO;

namespace MineLib.Network.Classic.Packets.Server
{
    public struct LevelDataChunkPacket : IPacketWithSize
    {
        public short ChunkLength;
        public byte[] ChunkData;
        public byte PercentComplete;

        public byte ID { get { return 0x03; } }
        public short Size { get { return 1028; } }

        public void ReadPacket(PacketByteReader stream)
        {
            ChunkLength = stream.ReadShort();
            ChunkData = stream.ReadByteArray(1024);
            PercentComplete = stream.ReadByte();
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteByte(ID);
            stream.WriteShort(ChunkLength);
            stream.WriteByteArray(ChunkData);
            stream.WriteByte(PercentComplete);
            stream.Purge();
        }
    }
}
