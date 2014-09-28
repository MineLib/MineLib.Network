using MineLib.Network.IO;
using MineLib.Network.Packets;

namespace MineLib.Network.Classic.Packets.Server
{
    public struct PositionUpdatePacket : IPacket
    {
        public sbyte PlayerID;
        public sbyte ChangeX;
        public sbyte ChangeY;
        public sbyte ChangeZ;

        public const byte PacketID = 0x0A;
        public byte ID { get { return PacketID; } }

        public void ReadPacket(PacketByteReader stream)
        {
            PlayerID = stream.ReadSByte();
            ChangeX = stream.ReadSByte();
            ChangeY = stream.ReadSByte();
            ChangeZ = stream.ReadSByte();
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteByte(ID);
            stream.WriteSByte(PlayerID);
            stream.WriteSByte(ChangeX);
            stream.WriteSByte(ChangeY);
            stream.WriteSByte(ChangeZ);
            stream.Purge();
        }
    }
}
