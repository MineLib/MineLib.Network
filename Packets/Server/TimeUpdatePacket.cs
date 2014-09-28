using MineLib.Network.IO;

namespace MineLib.Network.Packets.Server
{
    public struct TimeUpdatePacket : IPacket
    {
        public long AgeOfTheWorld;
        public long TimeOfDay;

        public byte ID { get { return 0x03; } }

        public void ReadPacket(PacketByteReader reader)
        {
            AgeOfTheWorld = reader.ReadLong();
            TimeOfDay = reader.ReadLong();
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(ID);
            stream.WriteLong(AgeOfTheWorld);
            stream.WriteLong(TimeOfDay);
            stream.Purge();
        }
    }
}