using MineLib.Network.IO;
using MineLib.Network.Data;

namespace MineLib.Network.Packets.Server
{
    public struct MultiBlockChangePacket : IPacket
    {
        public Coordinates2D Coordinates; // TODO: Add FromReader() ?
        public int RecordCount;
        private byte[] data;
        public RecordsArray RecordsArray;

        public const byte PacketID = 0x22;
        public byte Id { get { return PacketID; } }

        public void ReadPacket(PacketByteReader reader)
        {
            Coordinates.X = reader.ReadInt();
            Coordinates.Z = reader.ReadInt();
            RecordsArray = RecordsArray.FromReader(reader);
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(Id);
            stream.WriteInt(Coordinates.X);
            stream.WriteInt(Coordinates.Z);
            RecordsArray.ToStream(ref stream);
            stream.WriteVarInt(RecordCount);
            stream.WriteInt(RecordCount * 4);
            stream.WriteByteArray(data);
            stream.Purge();
        }
    }
}