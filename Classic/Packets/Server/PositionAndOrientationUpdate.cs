using MineLib.Network.IO;
using MineLib.Network.Packets;

namespace MineLib.Network.Classic.Packets.Server
{
    public struct PositionAndOrientationUpdatePacket : IPacket
    {
        public sbyte PlayerID;
        public sbyte ChangeX;
        public sbyte ChangeY;
        public sbyte ChangeZ;
        public byte Yaw;
        public byte Pitch;

        public const byte PacketID = 0x09;
        public byte ID { get { return PacketID; } }

        public void ReadPacket(PacketByteReader stream)
        {
            PlayerID = stream.ReadSByte();
            ChangeX = stream.ReadSByte();
            ChangeY = stream.ReadSByte();
            ChangeZ = stream.ReadSByte();
            Yaw = stream.ReadByte();
            Pitch = stream.ReadByte();
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteByte(ID);
            stream.WriteSByte(PlayerID);
            stream.WriteSByte(ChangeX);
            stream.WriteSByte(ChangeY);
            stream.WriteSByte(ChangeZ);
            stream.WriteByte(Yaw);
            stream.WriteByte(Pitch);
            stream.Purge();
        }
    }
}
