using MineLib.Network.IO;

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

        public const byte PacketID = 0x2D;
        public byte Id { get { return PacketID; } }

        public void ReadPacket(PacketByteReader stream)
        {
            WindowID = stream.ReadByte();
            InventoryType = stream.ReadByte();
            WindowTitle = stream.ReadString();
            NumberOfSlots = stream.ReadByte();
            UseProvidedTitle = stream.ReadBoolean();
            if (InventoryType == 11)
                EntityID = stream.ReadInt();
        }

        public void WritePacket(ref PacketStream stream)
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