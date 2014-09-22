using MineLib.Network.IO;

namespace MineLib.Network.Packets.Client
{
    public struct PlayerPositionPacket : IPacket
    {
        public double X, FeetY, Z;
        public bool OnGround;

        public const byte PacketID = 0x04;
        public byte Id { get { return PacketID; } }

        public void ReadPacket(PacketByteReader reader)
        {
            X = reader.ReadDouble();
            FeetY = reader.ReadDouble();
            Z = reader.ReadDouble();
            OnGround = reader.ReadBoolean();
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(Id);
            stream.WriteDouble(X);
            stream.WriteDouble(FeetY);
            stream.WriteDouble(Z);
            stream.WriteBoolean(OnGround);
            stream.Purge();
        }
    }
}