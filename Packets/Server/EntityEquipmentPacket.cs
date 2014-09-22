using MineLib.Network.IO;
using MineLib.Network.Data;
using MineLib.Network.Enums;

namespace MineLib.Network.Packets.Server
{
    public struct EntityEquipmentPacket : IPacket
    {
        public int EntityID;
        public EntityEquipmentSlot Slot;
        public ItemStack Item;

        public const byte PacketID = 0x04;
        public byte Id { get { return PacketID; } }

        public void ReadPacket(PacketByteReader reader)
        {
            EntityID = reader.ReadVarInt();
            Slot = (EntityEquipmentSlot) reader.ReadShort();
            Item = ItemStack.FromReader(reader);
        }
    
        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(Id);
            stream.WriteVarInt(EntityID);
            stream.WriteShort((short) Slot);
            Item.ToStream(ref stream);
            stream.Purge();
        }
    }
}