using MineLib.Network.Data;
using MineLib.Network.IO;

namespace MineLib.Network.Packets.Server
{
    public struct BlockActionPacket : IPacket
    {
        // Use BlockAction enum.
        public Vector3 Vector3;
        public byte Byte1;
        public byte Byte2;
        public int BlockType;

        public const byte PacketID = 0x24;
        public byte Id { get { return PacketID; } }

        public void ReadPacket(PacketByteReader stream)
        {
            Vector3.X = stream.ReadInt();
            Vector3.Y = stream.ReadShort();
            Vector3.Z = stream.ReadInt();
            Byte1 = stream.ReadByte();
            Byte2 = stream.ReadByte();
            BlockType = stream.ReadVarInt();
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(Id);
            stream.WriteInt((int)Vector3.X);
            stream.WriteShort((short)Vector3.Y);
            stream.WriteInt((int)Vector3.Z);
            stream.WriteByte(Byte1);
            stream.WriteByte(Byte2);
            stream.WriteVarInt(BlockType);
            stream.Purge();
        }
    }
}