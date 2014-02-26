using CWrapped;

namespace MineLib.Network.Packets.Server
{
    public struct OpenWindowPacket : IPacket
    {
        public byte WindowID;
        public byte InventoryType;
        public string WindowTitle;
        public byte NumberOfSlots;
        public bool UseProvidedTitle;
        public int? EntityID;

        public const byte PacketId = 0x2D;
        public byte Id { get { return 0x2D; } }

        public void ReadPacket(ref Wrapped stream)
        {
            WindowID = stream.ReadByte();
            InventoryType = stream.ReadByte();
            WindowTitle = stream.ReadString();
            NumberOfSlots = stream.ReadByte();
            UseProvidedTitle = stream.ReadBool();
            if (InventoryType == 11)
                EntityID = stream.ReadInt();
        }

        public void WritePacket(ref Wrapped stream)
        {
            stream.WriteVarInt(Id);
            stream.WriteByte(WindowID);
            stream.WriteByte(InventoryType);
            stream.WriteString(WindowTitle);
            stream.WriteByte(NumberOfSlots);
            stream.WriteBool(UseProvidedTitle);
            if (InventoryType == 11)
                stream.WriteInt(EntityID.GetValueOrDefault());
            stream.Purge();
        }
    }
}