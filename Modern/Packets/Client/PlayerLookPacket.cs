using MineLib.Network.IO;

namespace MineLib.Network.Modern.Packets.Client
{
    public struct PlayerLookPacket : IPacket
    {
        public float Yaw;
        public float Pitch;
        public bool OnGround;

        public byte ID { get { return 0x05; } }

        public IPacket ReadPacket(IMinecraftDataReader reader)
        {
            Yaw = reader.ReadFloat();
            Pitch = reader.ReadFloat();
            OnGround = reader.ReadBoolean();

            return this;
        }

        public IPacket WritePacket(IMinecraftStream stream)
        {
            stream.WriteVarInt(ID);
            stream.WriteFloat(Yaw);
            stream.WriteFloat(Pitch);
            stream.WriteBoolean(OnGround);
            stream.Purge();

            return this;
        }
    }
}