using MineLib.Network.IO;

namespace MineLib.Network.Classic.Packets.Extension.Server
{
    public struct CustomBlockSupportLevelPacket : IPacketWithSize
    {
        public byte SupportLevel;

        public byte ID { get { return 0x13; } }
        public short Size { get { return 2; } }

        public IPacketWithSize ReadPacket(MinecraftDataReader stream)
        {
            SupportLevel = stream.ReadByte();

            return this;
        }

        IPacket IPacket.ReadPacket(MinecraftDataReader stream)
        {
            return ReadPacket(stream);
        }

        public IPacket WritePacket(MinecraftStream stream)
        {
            stream.WriteByte(ID);
            stream.WriteByte(SupportLevel);
            stream.Purge();

            return this;
        }
    }
}
