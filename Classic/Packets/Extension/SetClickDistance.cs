using MineLib.Network.IO;
using MineLib.Network.Packets;

namespace MineLib.Network.Classic.Packets.Extension
{
    public struct SetClickDistancePacket : IPacket
    {
        public short Distance;

        public const byte PacketID = 0x12;
        public byte ID { get { return PacketID; } }

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
