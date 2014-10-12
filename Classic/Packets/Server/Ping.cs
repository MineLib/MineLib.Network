using MineLib.Network.IO;

namespace MineLib.Network.Classic.Packets.Server
{
    public struct PingPacket : IPacketWithSize
    {
        public byte ID { get { return 0x01; } }
        public short Size { get { return 1; } }

        public void ReadPacket(PacketByteReader stream)
        {
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteByte(ID);
            stream.Purge();
        }
    }
}
