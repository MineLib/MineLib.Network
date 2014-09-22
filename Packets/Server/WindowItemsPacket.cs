using MineLib.Network.IO;
using MineLib.Network.Data;

namespace MineLib.Network.Packets.Server
{
    public struct WindowItemsPacket : IPacket
    {
        public byte WindowId;
        public ItemStackArray SlotData;

        public const byte PacketID = 0x30;
        public byte Id { get { return PacketID; } }
    
        public void ReadPacket(PacketByteReader reader)
        {
            WindowId = reader.ReadByte();
            SlotData = ItemStackArray.FromReader(reader);
        }
    
        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(Id);
            stream.WriteByte(WindowId);
            SlotData.ToStream(ref stream);
            stream.Purge();
        }
    }
}