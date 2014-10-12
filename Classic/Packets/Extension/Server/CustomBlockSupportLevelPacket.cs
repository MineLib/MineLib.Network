using MineLib.Network.IO;

namespace MineLib.Network.Classic.Packets.Extension.Server
{
    public struct CustomBlockSupportLevelPacket : IPacketWithSize
    {
        public byte SupportLevel;

        public byte ID { get { return 0x13; } }
        public short Size { get { return 2; } }

        public void ReadPacket(PacketByteReader stream)
        {
            SupportLevel = stream.ReadByte();
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteByte(ID);
            stream.WriteByte(SupportLevel);
            stream.Purge();
        }
    }
}
