using MineLib.Network.IO;

namespace MineLib.Network.Modern.Packets.Client
{
    public struct PlayerPositionAndLookPacket : IPacket
    {
        public double X, FeetY, Z;
        public float Yaw;
        public float Pitch;
        public bool OnGround;

        public byte ID { get { return 0x06; } }

        public IPacket ReadPacket(MinecraftDataReader reader)
        {
            X = reader.ReadDouble();
            FeetY = reader.ReadDouble();
            Z = reader.ReadDouble();
            Yaw = reader.ReadFloat();
            Pitch = reader.ReadFloat();
            OnGround = reader.ReadBoolean();

            return this;
        }

        public IPacket WritePacket(MinecraftStream stream)
        {
            stream.WriteVarInt(ID);
            stream.WriteDouble(X);
            stream.WriteDouble(FeetY);
            stream.WriteDouble(Z);
            stream.WriteFloat(Yaw);
            stream.WriteFloat(Pitch);
            stream.WriteBoolean(OnGround);
            stream.Purge();

            return this;
        }
    }
}