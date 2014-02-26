using CWrapped;

namespace MineLib.Network.Packets.Server
{
    public struct MultiBlockChangeMetadata
    {
        public int BlockID;
        public int X, Y, Z;
        public int Metadata;
    }

    public struct MultiBlockChangePacket : IPacket
    {
        public int ChunkX, ChunkZ;
        public short RecordCount;
        public byte[] Data;
        public MultiBlockChangeMetadata[] Metadata;

        public const byte PacketId = 0x22;
        public byte Id { get { return 0x22; } }

        public void ReadPacket(ref Wrapped stream)
        {
            ChunkX = stream.ReadShort();
            ChunkZ = stream.ReadShort();
            RecordCount = stream.ReadShort();
            int size = stream.ReadInt();
            Data = stream.ReadByteArray(size);

            Metadata = new MultiBlockChangeMetadata[RecordCount - 1];

            //for (int i = 0; i < RecordCount - 1; i++)
            //{
            //    byte[] blockData = new byte[4];
            //    Buffer.BlockCopy(Data, (i * 4), blockData, 0, 4);
            //
            //    int metadata = blockData[3] & 0xF;
            //    Metadata[i].BlockID = (blockData[2] << 4) | ((blockData[3] & 0xF0) >> 4);
            //    Metadata[i].Y = (blockData[1]);
            //    Metadata[i].Z = (blockData[0] & 0x0f);
            //    Metadata[i].X = (blockData[0] >> 4) & 0x0f;
            //
            //    Metadata[i].X = (ChunkX * 16) + Metadata[i].X;
            //    Metadata[i].Z = (ChunkZ * 16) + Metadata[i].Z;
            //
            //}

        }

        public void WritePacket(ref Wrapped stream)
        {
            stream.WriteVarInt(Id);
            stream.WriteInt(ChunkX);
            stream.WriteInt(ChunkZ);
            stream.WriteShort(RecordCount);
            stream.WriteInt(RecordCount * 4);
            stream.WriteByteArray(Data);
            stream.Purge();
        }
    }
}