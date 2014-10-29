using MineLib.Network.IO;
using MineLib.Network.Modern.Data;

namespace MineLib.Network.Modern.Packets.Server
{
    public struct SetSlotPacket : IPacket
    {
        public sbyte WindowId;
        public short Slot;
        public ItemStack SlotData;

        public byte ID { get { return 0x2F; } }

        public IPacket ReadPacket(IMinecraftDataReader reader)
        {
            WindowId = reader.ReadSByte();
            Slot = reader.ReadShort();
            SlotData = ItemStack.FromReader(reader);

            return this;
        }
    
        public IPacket WritePacket(IMinecraftStream stream)
        {
            stream.WriteVarInt(ID);
            stream.WriteSByte(WindowId);
            stream.WriteShort(Slot);
            SlotData.ToStream(stream);
            stream.Purge();

            return this;
        }
    }
}