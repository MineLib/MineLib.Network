using MineLib.Network.IO;

namespace MineLib.Network.Modern.Packets.Server
{
    public struct TimeUpdatePacket : IPacket
    {
        public long AgeOfTheWorld;
        public long TimeOfDay;

        public byte ID { get { return 0x03; } }

        public IPacket ReadPacket(MinecraftDataReader reader)
        {
            AgeOfTheWorld = reader.ReadLong();
            TimeOfDay = reader.ReadLong();

            return this;
        }

        public IPacket WritePacket(MinecraftStream stream)
        {
            stream.WriteVarInt(ID);
            stream.WriteLong(AgeOfTheWorld);
            stream.WriteLong(TimeOfDay);
            stream.Purge();

            return this;
        }
    }
}