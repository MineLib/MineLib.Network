using MineLib.Network.IO;

namespace MineLib.Network.Main.Packets.Client
{
    public struct PlayerPositionAndLookPacket : IPacket
    {
        public double X, FeetY, Z;
        public float Yaw;
        public float Pitch;
        public bool OnGround;

        public byte ID { get { return 0x06; } }

        public void ReadPacket(PacketByteReader reader)
        {
            X = reader.ReadDouble();
            FeetY = reader.ReadDouble();
            Z = reader.ReadDouble();
            Yaw = reader.ReadFloat();
            Pitch = reader.ReadFloat();
            OnGround = reader.ReadBoolean();
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(ID);
            stream.WriteDouble(X);
            stream.WriteDouble(FeetY);
            stream.WriteDouble(Z);
            stream.WriteFloat(Yaw);
            stream.WriteFloat(Pitch);
            stream.WriteBoolean(OnGround);
            stream.Purge();
        }
    }
}