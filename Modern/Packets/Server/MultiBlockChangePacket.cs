using MineLib.Network.IO;
using MineLib.Network.Modern.Data;

namespace MineLib.Network.Modern.Packets.Server
{
    public struct MultiBlockChangePacket : IPacket
    {
        public Coordinates2D Coordinates; // TODO: Add FromReader() ?
        public RecordList RecordList;

        public byte ID { get { return 0x22; } }

        public IPacket ReadPacket(IMinecraftDataReader reader)
        {
            Coordinates.X = reader.ReadInt();
            Coordinates.Z = reader.ReadInt();
            RecordList = RecordList.FromReader(reader);

            return this;
        }

        public IPacket WritePacket(IMinecraftStream stream)
        {
            stream.WriteVarInt(ID);
            stream.WriteInt(Coordinates.X);
            stream.WriteInt(Coordinates.Z);
            RecordList.ToStream(stream);
            stream.Purge();

            return this;
        }
    }
}