using CWrapped;

namespace MineLib.Network.Packets.Client
{
    public struct PlayerLookPacket : IPacket
    {
        public float Yaw, Pitch;
        public bool OnGround;

        public const byte PacketID = 0x05;
        public byte Id { get { return PacketID; } }

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