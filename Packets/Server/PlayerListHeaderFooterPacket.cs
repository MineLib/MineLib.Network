using MineLib.Network.IO;

namespace MineLib.Network.Packets.Server
{
    public struct PlayerListHeaderFooterPacket : IPacket
    {
        public string Header;
        public string Footer;

        public const byte PacketID = 0x47;
        public byte Id { get { return PacketID; } }

        public void ReadPacket(PacketByteReader reader)
        {
            Header = reader.ReadString();
            Footer = reader.ReadString();
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(Id);
            stream.WriteString(Header);
            stream.WriteString(Footer);
            stream.Purge();
        }
    }
}
