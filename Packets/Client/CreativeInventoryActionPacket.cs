using MineLib.Network.IO;
using MineLib.Network.Data;

namespace MineLib.Network.Packets.Client
{
    public struct CreativeInventoryActionPacket : IPacket
    {
        public short Slot;
        public ItemStack ClickedItem;

        public const byte PacketID = 0x10;
        public byte Id { get { return PacketID; } }

        public void ReadPacket(PacketByteReader reader)
        {
            Slot = reader.ReadShort();
            ClickedItem = ItemStack.FromReader(reader);
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(Id);
            stream.WriteShort(Slot);
            ClickedItem.ToStream(ref stream);
            stream.Purge();
        }
    }
}