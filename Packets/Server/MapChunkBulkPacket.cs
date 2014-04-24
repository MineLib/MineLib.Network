using MineLib.Network.IO;

namespace MineLib.Network.Packets.Server
{
    public struct MapChunkBulkMetadata
    {
        public int ChunkX;
        public int ChunkZ;
        public short PrimaryBitMap;
        public short AddBitMap;
        public bool SkyLight;
    }

    public struct MapChunkBulkPacket : IPacket
    {
        public short ChunkColumnCount;
        public bool SkyLightSent;
        public byte[] ChunkData;
        public byte[] Trim;
        public MapChunkBulkMetadata[] MetaInformation;

        public const byte PacketID = 0x26;
        public byte Id { get { return PacketID; } }

        public void ReadPacket(PacketByteReader stream)
        {
            ChunkColumnCount = stream.ReadShort();
            var length = stream.ReadInt();
            SkyLightSent = stream.ReadBool();
            ChunkData = stream.ReadByteArray(length);
            Trim = new byte[length - 2];

            MetaInformation = new MapChunkBulkMetadata[ChunkColumnCount];
            for (int i = 0; i < ChunkColumnCount; i++)
            {
                var metadata = new MapChunkBulkMetadata
                {
                    ChunkX = stream.ReadInt(),
                    ChunkZ = stream.ReadInt(),
                    PrimaryBitMap = stream.ReadShort(),
                    AddBitMap = stream.ReadShort()
                };
                MetaInformation[i] = metadata;
            }

        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(Id);
            stream.WriteShort(ChunkColumnCount);
            stream.WriteInt(ChunkData.Length);
            stream.WriteBool(SkyLightSent);
            stream.WriteByteArray(ChunkData);

            for (int i = 0; i < ChunkColumnCount; i++)
            {
                stream.WriteInt(MetaInformation[i].ChunkX);
                stream.WriteInt(MetaInformation[i].ChunkZ);
                stream.WriteShort(MetaInformation[i].PrimaryBitMap);
                stream.WriteShort(MetaInformation[i].AddBitMap);
            }
            stream.Purge();
        }
    }
}