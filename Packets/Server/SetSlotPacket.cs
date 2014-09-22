using MineLib.Network.IO;
using MineLib.Network.Data;

namespace MineLib.Network.Packets.Server
{
    public struct SetSlotPacket : IPacket
    {
        public sbyte WindowId;
        public short Slot;
        public ItemStack SlotData;

        public const byte PacketID = 0x2F;
        public byte Id { get { return PacketID; } }

        public void ReadPacket(PacketByteReader reader)
        {
            WindowId = reader.ReadSByte();
            Slot = reader.ReadShort();
            SlotData = ItemStack.FromReader(reader);
        }
    
        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(Id);
            stream.WriteSByte(WindowId);
            stream.WriteShort(Slot);
            SlotData.ToStream(ref stream);
            stream.Purge();
        }
    }
}