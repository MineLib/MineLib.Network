using MineLib.Network.IO;

namespace MineLib.Network.Packets.Server
{
    public struct PlayerAbilitiesPacket : IPacket
    {
        public sbyte Flags;
        public float FlyingSpeed;
        public float WalkingSpeed;

        public byte ID { get { return 0x39; } }

        public void ReadPacket(PacketByteReader reader)
        {
            Flags = reader.ReadSByte();
            FlyingSpeed = reader.ReadFloat();
            WalkingSpeed = reader.ReadFloat();
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(ID);
            stream.WriteSByte(Flags);
            stream.WriteFloat(FlyingSpeed);
            stream.WriteFloat(WalkingSpeed);
            stream.Purge();
        }
    }
}