using CWrapped;

namespace MineLib.Network.Packets.Client
{
    public struct PlayerLookPacket : IPacket
    {
        public float Yaw, Pitch;
        public bool OnGround;

        public const byte PacketId = 0x0C;
        public byte Id { get { return 0x0C; } }

        public void ReadPacket(ref Wrapped stream)
        {
            Yaw = stream.ReadFloat();
            Pitch = stream.ReadFloat();
            OnGround = stream.ReadBool();
        }

        public void WritePacket(ref Wrapped stream)
        {
            stream.WriteVarInt(Id);
            stream.WriteFloat(Yaw);
            stream.WriteFloat(Pitch);
            stream.WriteBool(OnGround);
            stream.Purge();
        }
    }
}