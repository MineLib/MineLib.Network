using MineLib.Network.IO;

namespace MineLib.Network.Classic.Packets.Server
{
    public struct PingPacket : IPacketWithSize
    {
        public byte ID { get { return 0x01; } }
        public short Size { get { return 1; } }

        public IPacketWithSize ReadPacket(IMinecraftDataReader reader)
        {
            return this;
        }

        IPacket IPacket.ReadPacket(IMinecraftDataReader reader)
        {
            return ReadPacket(reader);
        }

        public IPacket WritePacket(IMinecraftStream stream)
        {
            stream.WriteByte(ID);
            stream.Purge();

            return this;
        }
    }
}
