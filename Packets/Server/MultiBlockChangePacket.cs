using System;
using MineLib.Network.IO;
using MineLib.Network.Data;

namespace MineLib.Network.Packets.Server
{
    public struct Records
    {
        public int BlockID;
        public Vector3 Vector3;
        public int Metadata;
    }

    public struct MultiBlockChangePacket : IPacket
    {
        // Implement FromStream and WriteTo for Records
        public Vector2 Vector2;
        public short RecordCount;
        public byte[] Data;
        public Records[] RecordsArray;

        public const byte PacketID = 0x22;
        public byte Id { get { return PacketID; } }

        public void ReadPacket(PacketByteReader stream)
        {
            Vector2.X = stream.ReadInt();
            Vector2.Z = stream.ReadInt();
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
                RecordsArray[i].Vector3.Y = (blockData[1]);
                RecordsArray[i].Vector3.Z = (blockData[0] & 0x0f);
                RecordsArray[i].Vector3.X = (blockData[0] >> 4) & 0x0f;

                RecordsArray[i].Vector3.X = ((int)Vector2.X * 16) + RecordsArray[i].Vector3.X;
                RecordsArray[i].Vector3.Z = ((int)Vector2.Z * 16) + RecordsArray[i].Vector3.Z;
            
            }

        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(Id);
            stream.WriteInt((int)Vector2.X);
            stream.WriteInt((int)Vector2.Z);
            stream.WriteShort(RecordCount);
            stream.WriteInt(RecordCount * 4);
            stream.WriteByteArray(Data);
            stream.Purge();
        }
    }
}