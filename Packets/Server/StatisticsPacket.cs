using MineLib.Network.Data;
using MineLib.Network.IO;

namespace MineLib.Network.Packets.Server
{
    public struct StatisticsPacket : IPacket
    {
        public StatisticsEntry Entries;

        public const byte PacketID = 0x37;
        public byte Id { get { return PacketID; } }

        public void ReadPacket(PacketByteReader reader)
        {
            Entries = StatisticsEntry.FromReader(reader);
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(Id);
            Entries.ToStream(ref stream);
            stream.Purge();
        }
    }
}