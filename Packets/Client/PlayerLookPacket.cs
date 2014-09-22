using MineLib.Network.IO;

namespace MineLib.Network.Packets.Client
{
    public struct PlayerLookPacket : IPacket
    {
        public float Yaw;
        public float Pitch;
        public bool OnGround;

        public const byte PacketID = 0x05;
        public byte Id { get { return PacketID; } }

        public void ReadPacket(PacketByteReader reader)
        {
            Yaw = reader.ReadFloat();
            Pitch = reader.ReadFloat();
            OnGround = reader.ReadBoolean();
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(Id);
            stream.WriteFloat(Yaw);
            stream.WriteFloat(Pitch);
            stream.WriteBoolean(OnGround);
            stream.Purge();
        }
    }
}