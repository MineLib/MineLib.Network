using MineLib.Network.IO;

namespace MineLib.Network.Modern.Packets.Server
{
    public struct PlayerListHeaderFooterPacket : IPacket
    {
        public string Header;
        public string Footer;

        public byte ID { get { return 0x47; } }

        public void ReadPacket(PacketByteReader reader)
        {
            Header = reader.ReadString();
            Footer = reader.ReadString();
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(ID);
            stream.WriteString(Header);
            stream.WriteString(Footer);
            stream.Purge();
        }
    }
}
