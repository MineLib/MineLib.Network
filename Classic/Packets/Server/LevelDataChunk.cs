using MineLib.Network.IO;
using MineLib.Network.Packets;

namespace MineLib.Network.Classic.Packets.Server
{
    public struct LevelDataChunkPacket : IPacket
    {
        public short ChunkLength;
        public byte[] ChunkData;
        public byte PercentComplete;

        public const byte PacketID = 0x03;
        public byte ID { get { return PacketID; } }

        public void ReadPacket(PacketByteReader stream)
        {
            ChunkLength = stream.ReadShort();
            ChunkData = stream.ReadByteArray(ChunkLength);
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
