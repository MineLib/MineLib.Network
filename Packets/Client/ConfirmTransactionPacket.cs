using CWrapped;
using MineLib.Network.Data;

namespace MineLib.Network.Packets.Client
{
    public struct ConfirmTransactionPacket : IPacket
    {
        public short Slot;
        public ItemStack ClickedItem;

        public const byte PacketId = 0x0F;
        public byte Id { get { return 0x0F; } }

        public void ReadPacket(ref Wrapped stream)
        {
            Slot = stream.ReadShort();
            ClickedItem = ItemStack.FromStream(ref stream);
        }

        public void WritePacket(ref Wrapped stream)
        {
            stream.WriteVarInt(Id);
            stream.WriteShort(Slot);
            ClickedItem.WriteTo(ref stream);
            stream.Purge();
        }
    }
}