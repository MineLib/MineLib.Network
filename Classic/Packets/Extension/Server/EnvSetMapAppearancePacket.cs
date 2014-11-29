using MineLib.Network.IO;

namespace MineLib.Network.Classic.Packets.Extension.Server
{
    public struct EnvSetMapAppearancePacket : IPacketWithSize
    {
        public string TextureURL;
        public byte SideBlock;
        public byte EdgeBlock;
        public short SideLevel;

        public byte ID { get { return 0x1E; } }
        public short Size { get { return 69; } }

        public IPacketWithSize ReadPacket(IMinecraftDataReader reader)
        {
            TextureURL = reader.ReadString();
            SideBlock = reader.ReadByte();
            EdgeBlock = reader.ReadByte();
            SideLevel = reader.ReadShort();

            return this;
        }

        IPacket IPacket.ReadPacket(IMinecraftDataReader reader)
        {
            return ReadPacket(reader);
        }

        public IPacket WritePacket(IMinecraftStream stream)
        {
            stream.WriteByte(ID);
            stream.WriteString(TextureURL);
            stream.WriteByte(SideBlock);
            stream.WriteByte(EdgeBlock);
            stream.WriteShort(SideLevel);
            stream.Purge();

            return this;
        }
    }
}
