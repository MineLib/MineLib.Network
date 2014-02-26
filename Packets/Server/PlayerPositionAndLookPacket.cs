using CWrapped;

namespace MineLib.Network.Packets.Server
{
    public struct PlayerPositionAndLookPacket : IPacket
    {
        public double X, Y, Z;
        public float Yaw, Pitch;
        public bool OnGround;

        public const byte PacketId = 0x08;
        public byte Id { get { return 0x08; } }

        public void ReadPacket(ref Wrapped stream)
        {
            X = stream.ReadDouble();
            Y = stream.ReadDouble();
            Z = stream.ReadDouble();
            Yaw = stream.ReadFloat();
            Pitch = stream.ReadFloat();
            OnGround = stream.ReadBool();
        }

        public void WritePacket(ref Wrapped stream)
        {
            stream.WriteVarInt(Id);
            stream.WriteDouble(X);
            stream.WriteDouble(Y);
            stream.WriteDouble(Z);
            stream.WriteFloat(Yaw);
            stream.WriteFloat(Pitch);
            stream.WriteBool(OnGround);
            stream.Purge();
        }
    }
}