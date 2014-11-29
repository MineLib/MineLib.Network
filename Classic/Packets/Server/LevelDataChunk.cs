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

        public IPacketWithSize ReadPacket(IMinecraftDataReader reader)
        {
            ChunkLength = reader.ReadShort();
            ChunkData = reader.ReadByteArray(1024);
            PercentComplete = reader.ReadByte();

            return this;
        }

        IPacket IPacket.ReadPacket(IMinecraftDataReader reader)
        {
            return ReadPacket(reader);
        }

        public IPacket WritePacket(IMinecraftStream stream)
        {
            stream.WriteByte(ID);
            stream.WriteShort(ChunkLength);
            stream.WriteByteArray(ChunkData);
            stream.WriteByte(PercentComplete);
            stream.Purge();

            return this;
        }
    }
}
