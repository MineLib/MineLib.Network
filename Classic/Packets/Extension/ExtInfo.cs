using MineLib.Network.IO;
using MineLib.Network.Packets;

namespace MineLib.Network.Classic.Packets.Extension
{
    public struct ExtInfoPacket : IPacket
    {
        public string AppName;
        public short ExtensionCount;

        public const byte PacketID = 0x10;
        public byte Id { get { return PacketID; } }

        public void ReadPacket(PacketByteReader stream)
        {
            AppName = stream.ReadString();
            ExtensionCount = stream.ReadShort();
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteByte(Id);
            stream.WriteString(AppName);
            stream.WriteShort(ExtensionCount);
            stream.Purge();
        }
    }
}
