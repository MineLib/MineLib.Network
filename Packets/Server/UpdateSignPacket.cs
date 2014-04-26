using MineLib.Network.Data;
using MineLib.Network.IO;

namespace MineLib.Network.Packets.Server
{
    public struct UpdateSignPacket : IPacket
    {
        public Coordinates3D Coordinates;
        public string[] Text;

        public const byte PacketID = 0x33;
        public byte Id { get { return PacketID; } }

        public void ReadPacket(PacketByteReader stream)
        {
            Coordinates.X = stream.ReadInt();
            Coordinates.Y = stream.ReadShort();
            Coordinates.Z = stream.ReadInt();
            Text = new string[3];
            Text[0] = stream.ReadString();
            Text[1] = stream.ReadString();
            Text[2] = stream.ReadString();
            Text[3] = stream.ReadString();
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(Id);
            stream.WriteInt(Coordinates.X);
            stream.WriteShort((short)Coordinates.Y);
            stream.WriteInt(Coordinates.Z);
            stream.WriteString(Text[0]);
            stream.WriteString(Text[1]);
            stream.WriteString(Text[2]);
            stream.WriteString(Text[3]);
            stream.Purge();
        }
    }
}