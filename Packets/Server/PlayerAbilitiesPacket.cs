using CWrapped;

namespace MineLib.Network.Packets.Server
{
    public struct PlayerAbilitiesPacket : IPacket
    {
        public byte Flags;
        public float FlyingSpeed, WalkingSpeed;

        public const byte PacketId = 0x39;
        public byte Id { get { return 0x39; } }

        public void ReadPacket(ref Wrapped stream)
        {
            Flags = stream.ReadByte();
            FlyingSpeed = stream.ReadFloat();
            WalkingSpeed = stream.ReadFloat();
        }

        public void WritePacket(ref Wrapped stream)
        {
            stream.WriteVarInt(Id);
            stream.WriteByte(Flags);
            stream.WriteFloat(FlyingSpeed);
            stream.WriteFloat(WalkingSpeed);
            stream.Purge();
        }
    }
}