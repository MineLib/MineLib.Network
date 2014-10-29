using MineLib.Network.IO;

namespace MineLib.Network.Modern.Packets.Server
{
    public struct PlayerListHeaderFooterPacket : IPacket
    {
        public string Header;
        public string Footer;

        public byte ID { get { return 0x47; } }

        public IPacket ReadPacket(IMinecraftDataReader reader)
        {
            Header = reader.ReadString();
            Footer = reader.ReadString();

            return this;
        }

        public IPacket WritePacket(IMinecraftStream stream)
        {
            stream.WriteVarInt(ID);
            stream.WriteString(Header);
            stream.WriteString(Footer);
            stream.Purge();

            return this;
        }
    }
}
