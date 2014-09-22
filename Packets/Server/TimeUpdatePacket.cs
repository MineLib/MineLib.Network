using MineLib.Network.IO;

namespace MineLib.Network.Packets.Server
{
    public struct TimeUpdatePacket : IPacket
    {
        public long AgeOfTheWorld;
        public long TimeOfDay;

        public const byte PacketID = 0x03;
        public byte Id { get { return PacketID; } }

        public void ReadPacket(PacketByteReader reader)
        {
            AgeOfTheWorld = reader.ReadLong();
            TimeOfDay = reader.ReadLong();
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(Id);
            stream.WriteLong(AgeOfTheWorld);
            stream.WriteLong(TimeOfDay);
            stream.Purge();
        }
    }
}