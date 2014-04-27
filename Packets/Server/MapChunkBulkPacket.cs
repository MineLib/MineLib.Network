using MineLib.Network.Data;
using MineLib.Network.IO;

namespace MineLib.Network.Packets.Server
{
    public struct ChunkColumnMetadata
    {
        public Coordinates2D Coordinates;
        public ushort PrimaryBitMap;
        public ushort AddBitMap;
        public bool SkyLightSend;
        public bool GroundUp; // True in 0x26
    }

    public struct MapChunkBulkPacket : IPacket
    {
        public short ChunkColumnCount;
        public bool SkyLightSent;
        public byte[] ChunkData;
        public ChunkColumnMetadata[] MetaInformation;

        public const byte PacketID = 0x26;
        public byte Id { get { return PacketID; } }

        public void ReadPacket(PacketByteReader stream)
        {
            ChunkColumnCount = stream.ReadShort();
            var length = stream.ReadInt();
            SkyLightSent = stream.ReadBoolean();
            ChunkData = stream.ReadByteArray(length);

            MetaInformation = new ChunkColumnMetadata[ChunkColumnCount];
            for (var i = 0; i < ChunkColumnCount; i++)
            {
                MetaInformation[i] = new ChunkColumnMetadata();
                MetaInformation[i].Coordinates.X = stream.ReadInt();
                MetaInformation[i].Coordinates.Z = stream.ReadInt();
                MetaInformation[i].PrimaryBitMap = stream.ReadUShort();
                MetaInformation[i].AddBitMap = stream.ReadUShort();
                MetaInformation[i].GroundUp = true;
                MetaInformation[i].SkyLightSend = SkyLightSent;
            }
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(Id);
            stream.WriteShort(ChunkColumnCount);
            stream.WriteInt(ChunkData.Length);
            stream.WriteBool(SkyLightSent);
            stream.WriteByteArray(ChunkData);

            for (var i = 0; i < ChunkColumnCount; i++)
            {
                stream.WriteInt(MetaInformation[i].Coordinates.X);
                stream.WriteInt(MetaInformation[i].Coordinates.Z);
                stream.WriteUShort(MetaInformation[i].PrimaryBitMap);
                stream.WriteUShort(MetaInformation[i].AddBitMap);
            }
            stream.Purge();
        }
    }
}