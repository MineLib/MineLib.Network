using MineLib.Network.IO;
using MineLib.Network.Enums;


namespace MineLib.Network.Packets.Client
{
    public struct ClientSettingsPacket : IPacket
    {
        public string Locale;
        public byte ViewDistance;
        public byte ChatFlags;
        public bool ChatColours;
        public Difficulty Difficulty;
        public bool ShowCape;

        public const byte PacketID = 0x15;
        public byte Id { get { return PacketID; } }

        public void ReadPacket(PacketByteReader stream)
        {
            Locale = stream.ReadString();
            ViewDistance = stream.ReadByte();
            ChatFlags = stream.ReadByte();
            ChatColours = stream.ReadBool();
            Difficulty = (Difficulty)stream.ReadByte();
            ShowCape = stream.ReadBool();
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(Id);
            stream.WriteString(Locale);
            stream.WriteByte(ViewDistance);
            stream.WriteByte(ChatFlags);
            stream.WriteBool(ChatColours);
            stream.WriteByte((byte)Difficulty);
            stream.WriteBool(ShowCape);
            stream.Purge();
        }
    }
}