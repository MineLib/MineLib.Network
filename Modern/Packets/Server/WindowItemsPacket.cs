using MineLib.Network.IO;
using MineLib.Network.Modern.Data;

namespace MineLib.Network.Modern.Packets.Server
{
    public struct WindowItemsPacket : IPacket
    {
        public byte WindowID;
        public ItemStackList ItemStackList;

        public byte ID { get { return 0x30; } }
    
        public IPacket ReadPacket(MinecraftDataReader reader)
        {
            WindowID = reader.ReadByte();
            ItemStackList = ItemStackList.FromReader(reader);

            return this;
        }
    
        public IPacket WritePacket(MinecraftStream stream)
        {
            stream.WriteVarInt(ID);
            stream.WriteByte(WindowID);
            ItemStackList.ToStream(ref stream);
            stream.Purge();

            return this;
        }
    }
}