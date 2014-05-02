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
    
        public void ReadPacket(PacketByteReader stream)
        {
            EntityID = stream.ReadInt();
            Slot = (EntityEquipmentSlot)stream.ReadShort();
            Item = ItemStack.FromReader(stream);
        }
    
        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(Id);
            stream.WriteInt(EntityID);
            stream.WriteShort((short)Slot);
            Item.WriteTo(ref stream);
            stream.Purge();
        }
    }
}