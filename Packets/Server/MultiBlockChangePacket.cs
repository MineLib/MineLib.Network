using System;
using MineLib.Network.IO;
using MineLib.Network.Data;

namespace MineLib.Network.Packets.Server
{
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
                RecordsArray[i].Y = (blockData[1]);
                RecordsArray[i].Z = (blockData[0] & 0x0f);
                RecordsArray[i].X = (blockData[0] >> 4) & 0x0f;

                RecordsArray[i].X = ((int)Vector2.X * 16) + RecordsArray[i].X;
                RecordsArray[i].Z = ((int)Vector2.Z * 16) + RecordsArray[i].Z;
            
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