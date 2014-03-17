using MineLib.Network.IO;
using MineLib.Network.Enums;


namespace MineLib.Network.Packets.Client
{
    public struct PlayerAbilitiesPacket : IPacket
    {
        public PlayerAbility Flags;
        public float FlyingSpeed, WalkingSpeed;

        public const byte PacketID = 0x13;
        public byte Id { get { return PacketID; } }

        public void ReadPacket(ref Wrapped stream)
        {
            Flags = (PlayerAbility)stream.ReadByte();
            FlyingSpeed = stream.ReadFloat();
            WalkingSpeed = stream.ReadFloat();
        }

        public void WritePacket(ref Wrapped stream)
        {
            stream.WriteVarInt(Id);
            stream.WriteByte((byte)Flags);
            stream.WriteFloat(FlyingSpeed);
            stream.WriteFloat(WalkingSpeed);
            stream.Purge();
        }
    }
}