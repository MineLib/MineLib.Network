using MineLib.Network.IO;

namespace MineLib.Network.Classic.Packets.Server
{
    public struct LevelInitializePacket : IPacketWithSize
    {
        public byte ID { get { return 0x02; } }
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
