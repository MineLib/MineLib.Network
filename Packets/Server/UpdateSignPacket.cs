using MineLib.Network.Data;
using MineLib.Network.IO;

namespace MineLib.Network.Packets.Server
{
    public struct UpdateSignPacket : IPacket
    {
        public Position Location;
        public string[] Text;

        public const byte PacketID = 0x33;
        public byte Id { get { return PacketID; } }

        public void ReadPacket(PacketByteReader reader)
        {
            Location = Position.FromReaderLong(reader);
            Text = new string[4];
            Text[0] = reader.ReadString();
            Text[1] = reader.ReadString();
            Text[2] = reader.ReadString();
            Text[3] = reader.ReadString();
           
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(Id);
            Location.ToStreamLong(ref stream);
            stream.WriteString(Text[0]);
            stream.WriteString(Text[1]);
            stream.WriteString(Text[2]);
            stream.WriteString(Text[3]);
            stream.Purge();
        }
    }
}