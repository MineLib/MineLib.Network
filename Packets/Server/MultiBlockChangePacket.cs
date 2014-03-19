using System;
using MineLib.Network.IO;
using MineLib.Network.Data;


namespace MineLib.Network.Packets.Server
{
    public struct MultiBlockChangePacket : IPacket
    {
        // Implement FromStream and WriteTo for Records
        public int ChunkX, ChunkZ;
        public short RecordCount;
        public byte[] Data;
        public Records[] RecordsArray;

        public const byte PacketID = 0x22;
        public byte Id { get { return PacketID; } }

        public void ReadPacket(PacketByteReader stream)
        {
            ChunkX = stream.ReadInt();
            ChunkZ = stream.ReadInt();
            RecordCount = stream.ReadShort();
            int size = stream.ReadInt();
            Data = stream.ReadByteArray(size);

            RecordsArray = new Records[RecordCount - 1];

            for (int i = 0; i < RecordCount - 1; i++)
            {
                var blockData = new byte[4];
                Buffer.BlockCopy(Data, (i * 4), blockData, 0, 4);

                RecordsArray[i].Metadata = blockData[3] & 0xF;
                RecordsArray[i].BlockID = (blockData[2] << 4) | ((blockData[3] & 0xF0) >> 4);
                RecordsArray[i].Y = (blockData[1]);
                RecordsArray[i].Z = (blockData[0] & 0x0f);
                RecordsArray[i].X = (blockData[0] >> 4) & 0x0f;

                RecordsArray[i].X = (ChunkX * 16) + RecordsArray[i].X;
                RecordsArray[i].Z = (ChunkZ * 16) + RecordsArray[i].Z;
            
            }

        }

        public void WritePacket(ref PacketStream stream)
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