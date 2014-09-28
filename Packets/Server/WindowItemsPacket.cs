using MineLib.Network.IO;
using MineLib.Network.Data;

namespace MineLib.Network.Packets.Server
{
    public struct WindowItemsPacket : IPacket
    {
        public byte WindowID;
        public ItemStackList ItemStackList;

        public byte ID { get { return 0x30; } }
    
        public void ReadPacket(PacketByteReader reader)
        {
            WindowID = reader.ReadByte();
            ItemStackList = ItemStackList.FromReader(reader);
        }
    
        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(ID);
            stream.WriteByte(WindowID);
            ItemStackList.ToStream(ref stream);
            stream.Purge();
        }
    }
}