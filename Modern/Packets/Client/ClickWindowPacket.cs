using MineLib.Network.Data.Structs;
using MineLib.Network.IO;

namespace MineLib.Network.Modern.Packets.Client
{
    public struct ClickWindowPacket : IPacket
    {
        public byte WindowID;
        public short Slot;
        public byte Button;
        public short ActionNumber;
        public byte Mode;
        public ItemStack ClickedItem;

        public byte ID { get { return 0x0E; } }

        public IPacket ReadPacket(IMinecraftDataReader reader)
        {
            WindowID = reader.ReadByte();
            Slot = reader.ReadShort();
            Button = reader.ReadByte();
            ActionNumber = reader.ReadShort();
            Mode = reader.ReadByte();
            ClickedItem = ItemStack.FromReader(reader);

            return this;
        }

        public IPacket WritePacket(IMinecraftStream stream)
        {
            stream.WriteVarInt(ID);
            stream.WriteByte(WindowID);
            stream.WriteShort(Slot);
            stream.WriteByte(Button);
            stream.WriteShort(ActionNumber);
            stream.WriteByte(Mode);
            ClickedItem.ToStream(stream);
            stream.Purge();

            return this;
        }
    }
}