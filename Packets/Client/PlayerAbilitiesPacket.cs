using MineLib.Network.IO;

namespace MineLib.Network.Packets.Client
{
    public struct PlayerAbilitiesPacket : IPacket
    {
        public byte Flags;
        public float FlyingSpeed;
        public float WalkingSpeed;

        public const byte PacketID = 0x13;
        public byte Id { get { return PacketID; } }

        public void ReadPacket(PacketByteReader reader)
        {
            Flags = reader.ReadByte();
            FlyingSpeed = reader.ReadFloat();
            WalkingSpeed = reader.ReadFloat();
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(Id);
            stream.WriteByte(Flags);
            stream.WriteFloat(FlyingSpeed);
            stream.WriteFloat(WalkingSpeed);
            stream.Purge();
        }
    }
}