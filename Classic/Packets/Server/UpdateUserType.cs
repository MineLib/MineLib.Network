using MineLib.Network.IO;
using MineLib.Network.Packets;

namespace MineLib.Network.Classic.Packets.Server
{
    public struct UpdateUserTypePacket : IPacket
    {
        public byte UserType;

        public const byte PacketID = 0x0F;
        public byte ID { get { return PacketID; } }

        public void ReadPacket(PacketByteReader stream)
        {
            UserType = stream.ReadByte();
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteByte(ID);
            stream.WriteByte(UserType);
            stream.Purge();
        }
    }
}
