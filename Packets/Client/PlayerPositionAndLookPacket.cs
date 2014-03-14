using CWrapped;

namespace MineLib.Network.Packets.Client
{
    public struct PlayerPositionAndLookPacket : IPacket
    {
        public double X, FeetY, HeadY, Z;
        public float Yaw, Pitch;
        public bool OnGround;

        public const byte PacketID = 0x06;
        public byte Id { get { return PacketID; } }

        public void ReadPacket(ref Wrapped stream)
        {
            X = stream.ReadDouble();
            FeetY = stream.ReadDouble();
            HeadY = stream.ReadDouble();
            Z = stream.ReadDouble();
            Yaw = stream.ReadFloat();
            Pitch = stream.ReadFloat();
            OnGround = stream.ReadBool();
        }

        public void WritePacket(ref Wrapped stream)
        {
            stream.WriteVarInt(Id);
            stream.WriteDouble(X);
            stream.WriteDouble(FeetY);
            stream.WriteDouble(HeadY);
            stream.WriteDouble(Z);
            stream.WriteFloat(Yaw);
            stream.WriteFloat(Pitch);
            stream.WriteBool(OnGround);
            stream.Purge();
        }
    }
}