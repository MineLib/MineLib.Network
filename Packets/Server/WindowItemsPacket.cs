using MineLib.Network.IO;
using MineLib.Network.Data;

namespace MineLib.Network.Packets.Server
{
    public struct WindowItemsPacket : IPacket
    {
        public byte WindowId;
        public ItemStack[] SlotData;

        public const byte PacketID = 0x30;
        public byte Id { get { return PacketID; } }
    
        public void ReadPacket(PacketByteReader stream)
        {
            WindowId = stream.ReadByte();
            short count = stream.ReadShort();
            SlotData = new ItemStack[count];
            for (int i = 0; i < count; i++)
                SlotData[i] = ItemStack.FromReader(stream);
        }
    
        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(Id);
            stream.WriteByte(WindowId);
            stream.WriteShort((short)SlotData.Length);
            for (int i = 0; i < SlotData.Length; i++)
                SlotData[i].WriteTo(ref stream);
            stream.Purge();
        }
    }
}