using MineLib.Network.IO;

namespace MineLib.Network.Classic.Packets.Extension.Client
{
    public struct ExtInfoPacket : IPacketWithSize
    {
        public string AppName;
        public short ExtensionCount;

        public byte ID { get { return 0x10; } }
        public short Size { get { return 67; } }

        public IPacketWithSize ReadPacket(IMinecraftDataReader reader)
        {
            AppName = reader.ReadString();
            ExtensionCount = reader.ReadShort();

            return this;
        }

        IPacket IPacket.ReadPacket(IMinecraftDataReader reader)
        {
            return ReadPacket(reader);
        }

        public IPacket WritePacket(IMinecraftStream stream)
        {
            stream.WriteByte(ID);
            stream.WriteString(AppName);
            stream.WriteShort(ExtensionCount);
            stream.Purge();

            return this;
        }
    }
}
