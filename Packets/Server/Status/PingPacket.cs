using MineLib.Network.IO;

namespace MineLib.Network.Packets.Server.Status
{
    public struct PingPacket : IPacket
    {
        public long Time;

        public const byte PacketID = 0x01;
        public byte Id { get { return PacketID; } }

        public void ReadPacket(PacketByteReader reader)
        {
            Time = reader.ReadLong();
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(Id);
            stream.WriteLong(Time);
            stream.Purge();
        }
    }
}