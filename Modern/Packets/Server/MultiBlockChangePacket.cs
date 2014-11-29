using MineLib.Network.Data;
using MineLib.Network.Data.Structs;
using MineLib.Network.IO;

namespace MineLib.Network.Modern.Packets.Server
{
    public struct MultiBlockChangePacket : IPacket
    {
        public Coordinates2D Coordinates; // TODO: Add FromReader() ?
        public RecordList RecordList;

        public byte ID { get { return 0x22; } }

        public IPacket ReadPacket(IMinecraftDataReader reader)
        {
            Coordinates = Coordinates2D.FromReaderInt(reader);
            RecordList = RecordList.FromReader(reader);

            return this;
        }

        public IPacket WritePacket(IMinecraftStream stream)
        {
            stream.WriteVarInt(ID);
            Coordinates.ToStreamInt(stream);
            RecordList.ToStream(stream);
            stream.Purge();

            return this;
        }
    }
}