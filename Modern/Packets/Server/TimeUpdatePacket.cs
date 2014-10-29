using MineLib.Network.IO;

namespace MineLib.Network.Modern.Packets.Server
{
    public struct TimeUpdatePacket : IPacket
    {
        public long AgeOfTheWorld;
        public long TimeOfDay;

        public byte ID { get { return 0x03; } }

        public IPacket ReadPacket(IMinecraftDataReader reader)
        {
            AgeOfTheWorld = reader.ReadLong();
            TimeOfDay = reader.ReadLong();

            return this;
        }

        public IPacket WritePacket(IMinecraftStream stream)
        {
            stream.WriteVarInt(ID);
            stream.WriteLong(AgeOfTheWorld);
            stream.WriteLong(TimeOfDay);
            stream.Purge();

            return this;
        }
    }
}