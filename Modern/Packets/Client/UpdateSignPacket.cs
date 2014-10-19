using MineLib.Network.IO;
using MineLib.Network.Modern.Data;

namespace MineLib.Network.Modern.Packets.Client
{
    public struct UpdateSignPacket : IPacket
    {
        public Position Location;
        public string[] Text;

        public byte ID { get { return 0x12; } }

        public IPacket ReadPacket(MinecraftDataReader reader)
        {
            Location = Position.FromReaderLong(reader);
            Text = new string[3];
            Text[0] = reader.ReadString();
            Text[1] = reader.ReadString();
            Text[2] = reader.ReadString();
            Text[3] = reader.ReadString();

            return this;
        }

        public IPacket WritePacket(MinecraftStream stream)
        {
            stream.WriteVarInt(ID);
            Location.ToStreamLong(stream);
            stream.WriteString(Text[0]);
            stream.WriteString(Text[1]);
            stream.WriteString(Text[2]);
            stream.WriteString(Text[3]);
            stream.Purge();

            return this;
        }
    }
}