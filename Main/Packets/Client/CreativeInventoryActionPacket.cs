using MineLib.Network.IO;
using MineLib.Network.Main.Data;

namespace MineLib.Network.Main.Packets.Client
{
    public struct CreativeInventoryActionPacket : IPacket
    {
        public short Slot;
        public ItemStack ClickedItem;

        public byte ID { get { return 0x10; } }

        public void ReadPacket(PacketByteReader reader)
        {
            Slot = reader.ReadShort();
            ClickedItem = ItemStack.FromReader(reader);
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(ID);
            stream.WriteShort(Slot);
            ClickedItem.ToStream(ref stream);
            stream.Purge();
        }
    }
}