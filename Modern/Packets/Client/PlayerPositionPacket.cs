using MineLib.Network.IO;

namespace MineLib.Network.Modern.Packets.Client
{
    public struct PlayerPositionPacket : IPacket
    {
        public double X, FeetY, Z;
        public bool OnGround;

        public byte ID { get { return 0x04; } }

        public IPacket ReadPacket(MinecraftDataReader reader)
        {
            X = reader.ReadDouble();
            FeetY = reader.ReadDouble();
            Z = reader.ReadDouble();
            OnGround = reader.ReadBoolean();

            return this;
        }

        public IPacket WritePacket(MinecraftStream stream)
        {
            stream.WriteVarInt(ID);
            stream.WriteDouble(X);
            stream.WriteDouble(FeetY);
            stream.WriteDouble(Z);
            stream.WriteBoolean(OnGround);
            stream.Purge();

            return this;
        }
    }
}