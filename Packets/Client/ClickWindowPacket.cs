using CWrapped;
using MineLib.Network.Data;

namespace MineLib.Network.Packets.Client
{
    public struct ClickWindowPacket : IPacket
    {
        public byte WindowID;
        public short Slot;
        public byte Button;
        public short ActionNumber;
        public byte Mode;
        public ItemStack ClickedItem;

        public const byte PacketID = 0x0E;
        public byte Id { get { return PacketID; } }

        public void ReadPacket(ref Wrapped stream)
        {
            WindowID = stream.ReadByte();
            Slot = stream.ReadShort();
            Button = stream.ReadByte();
            ActionNumber = stream.ReadShort();
            Mode = stream.ReadByte();
            ClickedItem = ItemStack.FromStream(ref stream);
        }

        public void WritePacket(ref Wrapped stream)
        {
            stream.WriteVarInt(Id);
            stream.WriteByte(WindowID);
            stream.WriteShort(Slot);
            stream.WriteByte(Button);
            stream.WriteShort(ActionNumber);
            stream.WriteByte(Mode);
            ClickedItem.WriteTo(ref stream);
            stream.Purge();
        }
    }
}