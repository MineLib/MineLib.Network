using MineLib.Network.IO;
using MineLib.Network.Modern.Data;

namespace MineLib.Network.Modern.Packets.Server
{
    public struct MultiBlockChangePacket : IPacket
    {
        public Coordinates2D Coordinates; // TODO: Add FromReader() ?
        public RecordList RecordList;

        public byte ID { get { return 0x22; } }

        public void ReadPacket(PacketByteReader reader)
        {
            Coordinates.X = reader.ReadInt();
            Coordinates.Z = reader.ReadInt();
            RecordList = RecordList.FromReader(reader);
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(ID);
            stream.WriteInt(Coordinates.X);
            stream.WriteInt(Coordinates.Z);
            RecordList.ToStream(ref stream);
            stream.Purge();
        }
    }
}