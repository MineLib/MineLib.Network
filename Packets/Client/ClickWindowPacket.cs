using MineLib.Network.IO;
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

        public void ReadPacket(PacketByteReader reader)
        {
            WindowID = reader.ReadByte();
            Slot = reader.ReadShort();
            Button = reader.ReadByte();
            ActionNumber = reader.ReadShort();
            Mode = reader.ReadByte();
            ClickedItem = ItemStack.FromReader(reader);
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(Id);
            stream.WriteByte(WindowID);
            stream.WriteShort(Slot);
            stream.WriteByte(Button);
            stream.WriteShort(ActionNumber);
            stream.WriteByte(Mode);
            ClickedItem.ToStream(ref stream);
            stream.Purge();
        }
    }
}