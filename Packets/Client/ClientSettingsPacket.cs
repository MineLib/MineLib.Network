using MineLib.Network.IO;

namespace MineLib.Network.Packets.Client
{
    public struct ClientSettingsPacket : IPacket
    {
        public string Locale;
        public byte ViewDistance;
        public byte ChatFlags;
        public bool ChatColours;
        public byte DisplayedSkinParts;

        public const byte PacketID = 0x15;
        public byte Id { get { return PacketID; } }

        public void ReadPacket(PacketByteReader reader)
        {
            Locale = reader.ReadString();
            ViewDistance = reader.ReadByte();
            ChatFlags = reader.ReadByte();
            ChatColours = reader.ReadBoolean();
            DisplayedSkinParts = reader.ReadByte();
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(Id);
            stream.WriteString(Locale);
            stream.WriteByte(ViewDistance);
            stream.WriteByte(ChatFlags);
            stream.WriteBoolean(ChatColours);
            stream.WriteByte(DisplayedSkinParts);
            stream.Purge();
        }
    }
}