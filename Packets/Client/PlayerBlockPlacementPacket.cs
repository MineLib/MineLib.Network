using MineLib.Network.IO;
using MineLib.Network.Data;
using MineLib.Network.Enums;

namespace MineLib.Network.Packets.Client
{
    public struct PlayerBlockPlacementPacket : IPacket
    {
        public Position Location;
        public Direction Direction;
        public ItemStack Slot;
        public Vector3 CursorVector3;

        public const byte PacketID = 0x08;
        public byte Id { get { return PacketID; } }

        public void ReadPacket(PacketByteReader reader)
        {
            Location = Position.FromReaderLong(reader);
            Direction = (Direction) reader.ReadByte();
            Slot = ItemStack.FromReader(reader);
            CursorVector3 = Vector3.FromReaderByte(reader);
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(Id);
            Location.ToStreamLong(ref stream);
            stream.WriteByte((byte) Direction);
            Slot.ToStream(ref stream);
            CursorVector3.ToStreamByte(ref stream);
            stream.Purge();
        }
    }
}