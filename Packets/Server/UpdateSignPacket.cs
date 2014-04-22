using MineLib.Network.Data;
using MineLib.Network.IO;

namespace MineLib.Network.Packets.Server
{
    public struct UpdateSignPacket : IPacket
    {
        public Vector3 Vector3;
        public string[] Text;

        public const byte PacketID = 0x33;
        public byte Id { get { return PacketID; } }

        public void ReadPacket(PacketByteReader stream)
        {
            Vector3.X = stream.ReadInt();
            Vector3.Y = stream.ReadShort();
            Vector3.Z = stream.ReadInt();
            Text = new string[3];
            Text[0] = stream.ReadString();
            Text[1] = stream.ReadString();
            Text[2] = stream.ReadString();
            Text[3] = stream.ReadString();
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(Id);
            stream.WriteInt((int)Vector3.X);
            stream.WriteShort((short)Vector3.Y);
            stream.WriteInt((int)Vector3.Z);
            stream.WriteString(Text[0]);
            stream.WriteString(Text[1]);
            stream.WriteString(Text[2]);
            stream.WriteString(Text[3]);
            stream.Purge();
        }
    }
}