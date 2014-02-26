using CWrapped;

namespace MineLib.Network.Packets.Client
{
    public struct PlayerPositionPacket : IPacket
    {
        public double X, Y, Stance, Z;
        public bool OnGround;

        public const byte PacketId = 0x0B;
        public byte Id { get { return 0x0B; } }

        public void ReadPacket(ref Wrapped stream)
        {
            X = stream.ReadDouble();
            Y = stream.ReadDouble();
            Stance = stream.ReadDouble();
            Z = stream.ReadDouble();
            OnGround = stream.ReadBool();
        }

        public void WritePacket(ref Wrapped stream)
        {
            stream.WriteVarInt(Id);
            stream.WriteDouble(X);
            stream.WriteDouble(Y);
            stream.WriteDouble(Stance);
            stream.WriteDouble(Z);
            stream.WriteBool(OnGround);
            stream.Purge();
        }
    }
}