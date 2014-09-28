using MineLib.Network.IO;

namespace MineLib.Network.Packets.Client
{
    public struct PlayerAbilitiesPacket : IPacket
    {
        public byte Flags;
        public float FlyingSpeed;
        public float WalkingSpeed;

        public byte ID { get { return 0x13; } }

        public void ReadPacket(PacketByteReader reader)
        {
            Flags = reader.ReadByte();
            FlyingSpeed = reader.ReadFloat();
            WalkingSpeed = reader.ReadFloat();
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(ID);
            stream.WriteByte(Flags);
            stream.WriteFloat(FlyingSpeed);
            stream.WriteFloat(WalkingSpeed);
            stream.Purge();
        }
    }
}