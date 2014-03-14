using CWrapped;

namespace MineLib.Network.Packets.Server
{
    public struct BlockActionPacket : IPacket
    {
        // Use BlockAction enum.
        public int X;
        public short Y;
        public int Z;
        public byte Byte1;
        public byte Byte2;
        public int BlockType;

        public const byte PacketID = 0x24;
        public byte Id { get { return PacketID; } }

        public void ReadPacket(ref Wrapped stream)
        {
            X = stream.ReadInt();
            Y = stream.ReadShort();
            Z = stream.ReadInt();
            Byte1 = stream.ReadByte();
            Byte2 = stream.ReadByte();
            BlockType = stream.ReadVarInt();
        }

        public void WritePacket(ref Wrapped stream)
        {
            stream.WriteVarInt(Id);
            stream.WriteInt(X);
            stream.WriteShort(Y);
            stream.WriteInt(Z);
            stream.WriteByte(Byte1);
            stream.WriteByte(Byte2);
            stream.WriteVarInt(BlockType);
            stream.Purge();
        }
    }
}