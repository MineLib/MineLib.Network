using MineLib.Network.IO;

namespace MineLib.Network.Classic.Packets.Extension.Server
{
    public struct SetClickDistancePacket : IPacketWithSize
    {
        public short Distance;

        public byte ID { get { return 0x12; } }
        public short Size { get { return 3; } }

        public IPacketWithSize ReadPacket(MinecraftDataReader stream)
        {
            Distance = stream.ReadShort();

            return this;
        }

        IPacket IPacket.ReadPacket(MinecraftDataReader stream)
        {
            return ReadPacket(stream);
        }

        public IPacket WritePacket(MinecraftStream stream)
        {
            stream.WriteByte(ID);
            stream.WriteShort(Distance);
            stream.Purge();

            return this;
        }
    }
}
