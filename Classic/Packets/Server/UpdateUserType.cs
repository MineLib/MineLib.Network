using MineLib.Network.Classic.Enums;
using MineLib.Network.IO;

namespace MineLib.Network.Classic.Packets.Server
{
    public struct UpdateUserTypePacket : IPacketWithSize
    {
        public UserType UserType;

        public byte ID { get { return 0x0F; } }
        public short Size { get { return 2; } }

        public IPacketWithSize ReadPacket(MinecraftDataReader stream)
        {
            UserType = (UserType) stream.ReadByte();

            return this;
        }

        IPacket IPacket.ReadPacket(MinecraftDataReader stream)
        {
            return ReadPacket(stream);
        }

        public IPacket WritePacket(MinecraftStream stream)
        {
            stream.WriteByte(ID);
            stream.WriteByte((byte) UserType);
            stream.Purge();

            return this;
        }
    }
}
