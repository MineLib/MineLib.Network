using System.Collections.Generic;
using MineLib.Network.IO;

namespace MineLib.Network.Data
{
    public struct Entry
    {
        public string StatisticsName;
        public int Value;
    }

    public class StatisticsEntry
    {
        private readonly List<Entry> _entries;

        public StatisticsEntry()
        {
            _entries = new List<Entry>();
        }

        public int Count
        {
            get { return _entries.Count; }
        }

        public Entry this[int index]
        {
            get { return _entries[index]; }
            set { _entries.Insert(index, value); }
        }

        public static StatisticsEntry FromReader(PacketByteReader reader)
        {
            var count = reader.ReadVarInt();

            var value = new StatisticsEntry();
            for (var i = 0; i < count; i++)
            {
                var entry = new Entry();

                entry.StatisticsName = reader.ReadString();
                entry.Value = reader.ReadVarInt();

                value[i] = entry;
            }

            return value;
        }

        public void ToStream(ref PacketStream stream)
        {
            stream.WriteVarInt(Count);

            foreach (var entry in _entries)
            {
                stream.WriteString(entry.StatisticsName);
                stream.WriteVarInt(entry.Value);
            }
        }
    }
}
