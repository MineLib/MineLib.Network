using MineLib.Network.IO;
using MineLib.Network.Packets;

namespace MineLib.Network.Classic.Packets.Server
{
    public struct OrientationUpdatePacket : IPacket
    {
        public sbyte PlayerID;
        public byte Yaw;
        public byte Pitch;

        public const byte PacketID = 0x0B;
        public byte ID { get { return PacketID; } }

        public void ReadPacket(PacketByteReader stream)
        {
            PlayerID = stream.ReadSByte();
            Yaw = stream.ReadByte();
            Pitch = stream.ReadByte();
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteByte(ID);
            stream.WriteSByte(PlayerID);
            stream.WriteByte(Yaw);
            stream.WriteByte(Pitch);
            stream.Purge();
        }
    }
}
