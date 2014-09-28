using MineLib.Network.IO;
using MineLib.Network.Packets;

namespace MineLib.Network.Classic.Packets.Extension
{
    public struct ExtEntryPacket : IPacket
    {
        public string ExtName;
        public int Version;

        public const byte PacketID = 0x11;
        public byte ID { get { return PacketID; } }

        public void ReadPacket(PacketByteReader stream)
        {
            ExtName = stream.ReadString();
            Version = stream.ReadInt();
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteByte(ID);
            stream.WriteString(ExtName);
            stream.WriteInt(Version);
            stream.Purge();
        }
    }
}
