using MineLib.Network.IO;
using MineLib.Network.Modern.Data;

namespace MineLib.Network.Modern.Packets.Server
{
    public struct StatisticsPacket : IPacket
    {
        public StatisticsEntryList StatisticsEntryList;

        public byte ID { get { return 0x37; } }

        public IPacket ReadPacket(MinecraftDataReader reader)
        {
            StatisticsEntryList = StatisticsEntryList.FromReader(reader);

            return this;
        }

        public IPacket WritePacket(MinecraftStream stream)
        {
            stream.WriteVarInt(ID);
            StatisticsEntryList.ToStream(stream);
            stream.Purge();

            return this;
        }
    }
}