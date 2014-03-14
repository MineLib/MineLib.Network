using CWrapped;

namespace MineLib.Network.Packets.Server.Status
{
    public struct PingPacket : IPacket
    {
        public long Time;

        public const byte PacketID = 0x01;
        public byte Id { get { return PacketID; } }

        public void ReadPacket(ref Wrapped stream)
        {
            Time = stream.ReadLong();
        }

        public void WritePacket(ref Wrapped stream)
        {
            stream.WriteVarInt(Id);
            stream.WriteLong(Time);
            stream.Purge();
        }
    }
}