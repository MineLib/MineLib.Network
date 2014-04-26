using System;
using MineLib.Network.IO;
using MineLib.Network.Data;

namespace MineLib.Network.Packets.Server
{
    public struct Records
    {
        public int BlockID;
        public Coordinates3D Coordinates;
        public int Metadata;
    }

    public struct MultiBlockChangePacket : IPacket
    {
        // Implement FromStream and WriteTo for Records
        public Coordinates2D Coordinates;
        public short RecordCount;
        public byte[] Data;
        public Records[] RecordsArray;

        public const byte PacketID = 0x22;
        public byte Id { get { return PacketID; } }

        public void ReadPacket(PacketByteReader stream)
        {
            Coordinates.X = stream.ReadInt();
            Coordinates.Z = stream.ReadInt();
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
                RecordsArray[i].Coordinates.Y = (blockData[1]);
                RecordsArray[i].Coordinates.Z = (blockData[0] & 0x0f);
                RecordsArray[i].Coordinates.X = (blockData[0] >> 4) & 0x0f;

                RecordsArray[i].Coordinates.X = (Coordinates.X * 16) + RecordsArray[i].Coordinates.X;
                RecordsArray[i].Coordinates.Z = (Coordinates.Z * 16) + RecordsArray[i].Coordinates.Z;
            
            }

        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(Id);
            stream.WriteInt(Coordinates.X);
            stream.WriteInt(Coordinates.Z);
            stream.WriteShort(RecordCount);
            stream.WriteInt(RecordCount * 4);
            stream.WriteByteArray(Data);
            stream.Purge();
        }
    }
}