using MineLib.Network.IO;

namespace MineLib.Network.Packets.Client
{
    public struct PlayerPositionPacket : IPacket
    {
        public double X, FeetY, HeadY, Z;
        public bool OnGround;

        public const byte PacketID = 0x04;
        public byte Id { get { return PacketID; } }

        public void ReadPacket(PacketByteReader stream)
        {
            X = stream.ReadDouble();
            FeetY = stream.ReadDouble();
            HeadY = stream.ReadDouble();
            Z = stream.ReadDouble();
            OnGround = stream.ReadBool();
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(Id);
            stream.WriteDouble(X);
            stream.WriteDouble(FeetY);
            stream.WriteDouble(HeadY);
            stream.WriteDouble(Z);
            stream.WriteBool(OnGround);
            stream.Purge();
        }
    }
}