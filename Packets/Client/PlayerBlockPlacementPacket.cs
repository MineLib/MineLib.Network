using MineLib.Network.IO;
using MineLib.Network.Data;
using MineLib.Network.Enums;


namespace MineLib.Network.Packets.Client
{
    public struct PlayerBlockPlacementPacket : IPacket
    {
        public int X;
        public byte Y;
        public int Z;
        public Direction Direction;
        public ItemStack Slot;
        public byte CursorX, CursorY, CursorZ;

        public const byte PacketID = 0x08;
        public byte Id { get { return PacketID; } }

        public void ReadPacket(PacketByteReader stream)
        {
            X = stream.ReadInt();
            Y = stream.ReadByte();
            Z = stream.ReadInt();
            Direction = (Direction)stream.ReadByte();
            Slot = ItemStack.FromStream(ref stream);
            CursorX = stream.ReadByte();
            CursorY = stream.ReadByte();
            CursorZ = stream.ReadByte();
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(Id);
            stream.WriteInt(X);
            stream.WriteByte(Y);
            stream.WriteInt(Z);
            stream.WriteByte((byte)Direction);
            Slot.WriteTo(ref stream);
            stream.WriteByte(CursorX);
            stream.WriteByte(CursorY);
            stream.WriteByte(CursorZ);
            stream.Purge();
        }
    }
}