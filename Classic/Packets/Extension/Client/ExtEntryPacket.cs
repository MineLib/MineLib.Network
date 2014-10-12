using MineLib.Network.IO;

namespace MineLib.Network.Classic.Packets.Extension.Client
{
    public struct ExtEntryPacket : IPacketWithSize
    {
        public string ExtName;
        public int Version;

        public byte ID { get { return 0x11; } }
        public short Size { get { return 69; } }

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
