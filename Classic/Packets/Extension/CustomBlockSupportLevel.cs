using MineLib.Network.IO;
using MineLib.Network.Packets;

namespace MineLib.Network.Classic.Packets.Extension
{
    public struct CustomBlockSupportLevelPacket : IPacket
    {
        public byte SupportLevel;

        public const byte PacketID = 0x13;
        public byte ID { get { return PacketID; } }

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
