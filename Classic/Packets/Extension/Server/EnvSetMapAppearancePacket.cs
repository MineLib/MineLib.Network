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

        public void ReadPacket(PacketByteReader stream)
        {
            TextureURL = stream.ReadString();
            SideBlock = stream.ReadByte();
            EdgeBlock = stream.ReadByte();
            SideLevel = stream.ReadShort();
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteByte(ID);
            stream.WriteString(TextureURL);
            stream.WriteByte(SideBlock);
            stream.WriteByte(EdgeBlock);
            stream.WriteShort(SideLevel);
            stream.Purge();
        }
    }
}
