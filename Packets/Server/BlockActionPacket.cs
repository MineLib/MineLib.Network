using MineLib.Network.Data;
using MineLib.Network.IO;

namespace MineLib.Network.Packets.Server
{
    public struct BlockActionPacket : IPacket
    {
        // Use BlockAction enum.
        public Coordinates3D Coordinates;
        public byte Byte1;
        public byte Byte2;
        public int BlockType;

        public const byte PacketID = 0x24;
        public byte Id { get { return PacketID; } }

        public void ReadPacket(PacketByteReader stream)
        {
            Coordinates.X = stream.ReadInt();
            Coordinates.Y = stream.ReadShort();
            Coordinates.Z = stream.ReadInt();
            Byte1 = stream.ReadByte();
            Byte2 = stream.ReadByte();
            BlockType = stream.ReadVarInt();
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(Id);
            stream.WriteInt(Coordinates.X);
            stream.WriteShort((short)Coordinates.Y);
            stream.WriteInt(Coordinates.Z);
            stream.WriteByte(Byte1);
            stream.WriteByte(Byte2);
            stream.WriteVarInt(BlockType);
            stream.Purge();
        }
    }
}