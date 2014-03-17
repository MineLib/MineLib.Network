using MineLib.Network.IO;
using MineLib.Network.Data;


namespace MineLib.Network.Packets.Client
{
    public struct ConfirmTransactionPacket : IPacket
    {
        public short Slot;
        public ItemStack ClickedItem;

        public const byte PacketID = 0x0F;
        public byte Id { get { return PacketID; } }

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