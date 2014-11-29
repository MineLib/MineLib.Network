using MineLib.Network.Data;
using MineLib.Network.IO;

namespace MineLib.Network.Classic.Packets.Extension.Server
{
    public struct MakeSelectionPacket : IPacketWithSize
    {
        public byte SelectionID;
        public string Label;
        public Position StartLocation;
        public Position EndLocation;
        public short Red;
        public short Green;
        public short Blue;
        public short Opacity;

        public byte ID { get { return 0x1A; } }
        public short Size { get { return 86; } }

        public IPacketWithSize ReadPacket(IMinecraftDataReader reader)
        {
            SelectionID = reader.ReadByte();
            Label = reader.ReadString();
            StartLocation = Position.FromReaderShort(reader);
            EndLocation = Position.FromReaderShort(reader);
            Red = reader.ReadShort();
            Green = reader.ReadShort();
            Blue = reader.ReadShort();
            Opacity = reader.ReadShort();

            return this;
        }

        IPacket IPacket.ReadPacket(IMinecraftDataReader reader)
        {
            return ReadPacket(reader);
        }

        public IPacket WritePacket(IMinecraftStream stream)
        {
            stream.WriteByte(ID);
            stream.WriteByte(SelectionID);
            stream.WriteString(Label);
            StartLocation.ToStreamShort(stream);
            EndLocation.ToStreamShort(stream);
            stream.WriteShort(Red);
            stream.WriteShort(Green);
            stream.WriteShort(Blue);
            stream.WriteShort(Opacity);
            stream.Purge();

            return this;
        }
    }
}
