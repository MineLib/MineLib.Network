using MineLib.Network.IO;

namespace MineLib.Network.Modern.Packets.Client
{
    public struct PlayerAbilitiesPacket : IPacket
    {
        public byte Flags;
        public float FlyingSpeed;
        public float WalkingSpeed;

        public byte ID { get { return 0x13; } }

        public IPacket ReadPacket(IMinecraftDataReader reader)
        {
            Flags = reader.ReadByte();
            FlyingSpeed = reader.ReadFloat();
            WalkingSpeed = reader.ReadFloat();

            return this;
        }

        public IPacket WritePacket(IMinecraftStream stream)
        {
            stream.WriteVarInt(ID);
            stream.WriteByte(Flags);
            stream.WriteFloat(FlyingSpeed);
            stream.WriteFloat(WalkingSpeed);
            stream.Purge();

            return this;
        }
    }
}