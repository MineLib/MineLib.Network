using MineLib.Network.IO;

namespace MineLib.Network.Main.Packets.Client
{
    public struct PlayerLookPacket : IPacket
    {
        public float Yaw;
        public float Pitch;
        public bool OnGround;

        public byte ID { get { return 0x05; } }

        public void ReadPacket(PacketByteReader reader)
        {
            Yaw = reader.ReadFloat();
            Pitch = reader.ReadFloat();
            OnGround = reader.ReadBoolean();
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(ID);
            stream.WriteFloat(Yaw);
            stream.WriteFloat(Pitch);
            stream.WriteBoolean(OnGround);
            stream.Purge();
        }
    }
}