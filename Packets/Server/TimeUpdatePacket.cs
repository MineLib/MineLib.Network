using MineLib.Network.IO;


namespace MineLib.Network.Packets.Server
{
    public struct TimeUpdatePacket : IPacket
    {
        public long AgeOfTheWorld, TimeOfDay;

        public const byte PacketID = 0x03;
        public byte Id { get { return PacketID; } }

        public void ReadPacket(ref Wrapped stream)
        {
            AgeOfTheWorld = stream.ReadLong();
            TimeOfDay = stream.ReadLong();
        }

        public void WritePacket(ref Wrapped stream)
        {
            stream.WriteVarInt(Id);
            stream.WriteLong(AgeOfTheWorld);
            stream.WriteLong(TimeOfDay);
            stream.Purge();
        }
    }
}