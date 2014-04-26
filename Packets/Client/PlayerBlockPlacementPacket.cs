using MineLib.Network.IO;
using MineLib.Network.Data;
using MineLib.Network.Enums;

namespace MineLib.Network.Packets.Client
{
    public struct PlayerBlockPlacementPacket : IPacket
    {
        public Coordinates3D Coordinates;
        public Direction Direction;
        public ItemStack Slot;
        public Vector3 CursorVector3;

        public const byte PacketID = 0x08;
        public byte Id { get { return PacketID; } }

        public void ReadPacket(PacketByteReader stream)
        {
            Coordinates.X = stream.ReadInt();
            Coordinates.Y = stream.ReadByte();
            Coordinates.Z = stream.ReadInt();
            Direction = (Direction)stream.ReadByte();
            Slot = ItemStack.FromStream(ref stream);
            CursorVector3.X = stream.ReadByte();
            CursorVector3.Y = stream.ReadByte();
            CursorVector3.Z = stream.ReadByte();
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(Id);
            stream.WriteInt(Coordinates.X);
            stream.WriteByte((byte)Coordinates.Y);
            stream.WriteInt(Coordinates.Z);
            stream.WriteByte((byte)Direction);
            Slot.WriteTo(ref stream);
            stream.WriteByte((byte)CursorVector3.X);
            stream.WriteByte((byte)CursorVector3.Y);
            stream.WriteByte((byte)CursorVector3.Z);
            stream.Purge();
        }
    }
}