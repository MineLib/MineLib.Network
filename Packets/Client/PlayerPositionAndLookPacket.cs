using MineLib.Network.IO;

namespace MineLib.Network.Packets.Client
{
    public struct PlayerPositionAndLookPacket : IPacket
    {
        public double X, FeetY, HeadY, Z;
        public float Yaw;
        public float Pitch;
        public bool OnGround;

        public const byte PacketID = 0x06;
        public byte Id { get { return PacketID; } }

        public void ReadPacket(PacketByteReader reader)
        {
            X = reader.ReadDouble();
            FeetY = reader.ReadDouble();
            HeadY = reader.ReadDouble();
            Z = reader.ReadDouble();
            Yaw = reader.ReadFloat();
            Pitch = reader.ReadFloat();
            OnGround = reader.ReadBoolean();
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(Id);
            stream.WriteDouble(X);
            stream.WriteDouble(FeetY);
            stream.WriteDouble(HeadY);
            stream.WriteDouble(Z);
            stream.WriteFloat(Yaw);
            stream.WriteFloat(Pitch);
            stream.WriteBoolean(OnGround);
            stream.Purge();
        }
    }
}