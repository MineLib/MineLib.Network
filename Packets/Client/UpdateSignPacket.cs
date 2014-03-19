using MineLib.Network.IO;


namespace MineLib.Network.Packets.Client
{
    public struct UpdateSignPacket : IPacket
    {
        public int X;
        public short Y;
        public int Z;
        public string Line1, Line2, Line3, Line4;

        public const byte PacketID = 0x12;
        public byte Id { get { return PacketID; } }

        public void ReadPacket(PacketByteReader stream)
        {
            X = stream.ReadInt();
            Y = stream.ReadShort();
            Z = stream.ReadInt();
            Line1 = stream.ReadString();
            Line2 = stream.ReadString();
            Line3 = stream.ReadString();
            Line4 = stream.ReadString();
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(Id);
            stream.WriteInt(X);
            stream.WriteShort(Y);
            stream.WriteInt(Z);
            stream.WriteString(Line1);
            stream.WriteString(Line2);
            stream.WriteString(Line3);
            stream.WriteString(Line4);
            stream.Purge();
        }
    }
}