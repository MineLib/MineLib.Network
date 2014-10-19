using MineLib.Network.IO;

namespace MineLib.Network.Modern.Packets.Server
{
    public struct OpenWindowPacket : IPacket
    {
        public byte WindowID;
        public string InventoryType;
        public string WindowTitle;
        public byte NumberOfSlots;
        public int? EntityID;

        public byte ID { get { return 0x2D; } }

        public IPacket ReadPacket(MinecraftDataReader reader)
        {
            WindowID = reader.ReadByte();
            InventoryType = reader.ReadString();
            WindowTitle = reader.ReadString();
            NumberOfSlots = reader.ReadByte();
            if (InventoryType == "EntityHorse")
                EntityID = reader.ReadInt();

            return this;
        }

        public IPacket WritePacket(MinecraftStream stream)
        {
            stream.WriteVarInt(ID);
            stream.WriteByte(WindowID);
            stream.WriteString(InventoryType);
            stream.WriteString(WindowTitle);
            stream.WriteByte(NumberOfSlots);
            if (InventoryType == "EntityHorse")
                stream.WriteInt(EntityID.GetValueOrDefault());
            stream.Purge();

            return this;
        }
    }
}