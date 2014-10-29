using MineLib.Network.IO;

namespace MineLib.Network.Modern.Packets.Server
{
    public struct ResourcePackSendPacket : IPacket
    {
        public string URL;
        public string Hash;

        public byte ID { get { return 0x48; } }

        public IPacket ReadPacket(IMinecraftDataReader reader)
        {
            URL = reader.ReadString();
            Hash = reader.ReadString();

            return this;
        }

        public IPacket WritePacket(IMinecraftStream stream)
        {
            stream.WriteVarInt(ID);
            stream.WriteString(URL);
            stream.WriteString(Hash);
            stream.Purge();

            return this;
        }
    }
}
