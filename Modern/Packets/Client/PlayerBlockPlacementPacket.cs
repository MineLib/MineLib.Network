using MineLib.Network.IO;
using MineLib.Network.Modern.Data;
using MineLib.Network.Modern.Enums;

namespace MineLib.Network.Modern.Packets.Client
{
    public struct PlayerBlockPlacementPacket : IPacket
    {
        public Position Location;
        public Direction Direction;
        public ItemStack Slot;
        public Vector3 CursorVector3;

        public byte ID { get { return 0x08; } }

        public void ReadPacket(PacketByteReader reader)
        {
            Location = Position.FromReaderLong(reader);
            Direction = (Direction) reader.ReadByte();
            Slot = ItemStack.FromReader(reader);
            CursorVector3 = Vector3.FromReaderByte(reader);
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(ID);
            Location.ToStreamLong(ref stream);
            stream.WriteByte((byte) Direction);
            Slot.ToStream(ref stream);
            CursorVector3.ToStreamByte(ref stream);
            stream.Purge();
        }
    }
}