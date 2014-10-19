using MineLib.Network.IO;
using MineLib.Network.Modern.Data;

namespace MineLib.Network.Modern.Packets.Client
{
    public struct CreativeInventoryActionPacket : IPacket
    {
        public short Slot;
        public ItemStack ClickedItem;

        public byte ID { get { return 0x10; } }

        public IPacket ReadPacket(MinecraftDataReader reader)
        {
            Slot = reader.ReadShort();
            ClickedItem = ItemStack.FromReader(reader);

            return this;
        }

        public IPacket WritePacket(MinecraftStream stream)
        {
            stream.WriteVarInt(ID);
            stream.WriteShort(Slot);
            ClickedItem.ToStream(stream);
            stream.Purge();

            return this;
        }
    }
}