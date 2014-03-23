using MineLib.Network.IO;


namespace MineLib.Network.Packets.Server
{
    public struct UpdateSignPacket : IPacket
    {
        public int X;
        public short Y;
        public int Z;
        public string[] Text;

        public const byte PacketID = 0x33;
        public byte Id { get { return PacketID; } }

        public void ReadPacket(PacketByteReader stream)
        {
            Text = new string[4];

            X = stream.ReadInt();
            Y = stream.ReadShort();
            Z = stream.ReadInt();
            Text[0] = stream.ReadString();
            Text[1] = stream.ReadString();
            Text[2] = stream.ReadString();
            Text[3] = stream.ReadString();
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(Id);
            stream.WriteInt(X);
            stream.WriteShort(Y);
            stream.WriteInt(Z);
            stream.WriteString(Text[0]);
            stream.WriteString(Text[1]);
            stream.WriteString(Text[2]);
            stream.WriteString(Text[3]);
            stream.Purge();
        }
    }
}