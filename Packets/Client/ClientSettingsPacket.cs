using CWrapped;

namespace MineLib.Network.Packets.Client
{
    public struct ClientSettingsPacket : IPacket
    {
        public string Locale;
        public byte ViewDistance;
        public byte ChatFlags;
        public bool ChatColours;
        public byte Difficulty;
        public bool ShowCape;

        public const byte PacketId = 0x15;
        public byte Id { get { return 0x15; } }

        public void ReadPacket(ref Wrapped stream)
        {
            Locale = stream.ReadString();
            ViewDistance = stream.ReadByte();
            ChatFlags = stream.ReadByte();
            ChatColours = stream.ReadBool();
            Difficulty = stream.ReadByte();
            ShowCape = stream.ReadBool();
        }

        public void WritePacket(ref Wrapped stream)
        {
            stream.WriteVarInt(Id);
            stream.WriteString(Locale);
            stream.WriteByte(ViewDistance);
            stream.WriteByte(ChatFlags);
            stream.WriteBool(ChatColours);
            stream.WriteByte(Difficulty);
            stream.WriteBool(ShowCape);
            stream.Purge();
        }
    }
}