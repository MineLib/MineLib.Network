using MineLib.Network.IO;

namespace MineLib.Network.Classic.Packets.Extension.Client
{
    public struct ExtInfoPacket : IPacketWithSize
    {
        public string AppName;
        public short ExtensionCount;

        public byte ID { get { return 0x10; } }
        public short Size { get { return 67; } }

        public void ReadPacket(PacketByteReader stream)
        {
            AppName = stream.ReadString();
            ExtensionCount = stream.ReadShort();
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteByte(ID);
            stream.WriteString(AppName);
            stream.WriteShort(ExtensionCount);
            stream.Purge();
        }
    }
}
