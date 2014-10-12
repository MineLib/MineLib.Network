using MineLib.Network.IO;

namespace MineLib.Network.Classic.Packets.Extension.Server
{
    public struct SetClickDistancePacket : IPacketWithSize
    {
        public short Distance;

        public byte ID { get { return 0x12; } }
        public short Size { get { return 3; } }

        public void ReadPacket(PacketByteReader stream)
        {
            Distance = stream.ReadShort();
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteByte(ID);
            stream.WriteShort(Distance);
            stream.Purge();
        }
    }
}
