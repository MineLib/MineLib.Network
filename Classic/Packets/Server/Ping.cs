using MineLib.Network.IO;
using MineLib.Network.Packets;

namespace MineLib.Network.Classic.Packets.Server
{
    public struct PingPacket : IPacket
    {
        public const byte PacketID = 0x01;
        public byte ID { get { return PacketID; } }

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
