using System.Collections.Generic;
using MineLib.Network.IO;

namespace MineLib.Network.Data.Structs
{
    public struct Record
    {
        public int BlockIDMeta;
        public Position Coordinates;
    }

    // TODO: Records bitmask
    public class RecordList
    {
        private readonly List<Record> _entries;

        public RecordList()
        {
            _entries = new List<Record>();
        }

        public int Count
        {
            get { return _entries.Count; }
        }

        public Record this[int index]
        {
            get { return _entries[index]; }
            set { _entries.Insert(index, value); }
        }

        public static RecordList FromReader(IMinecraftDataReader reader)
        {
            var value = new RecordList();

            var count = reader.ReadVarInt();

            for (int i = 0; i < count; i++)
            {
                var record = new Record();

                var coordinates = reader.ReadShort();
                var y = coordinates & 0xFF;
                var z = (coordinates >> 8) & 0xf; 
                var x = (coordinates >> 12) & 0xf;

                record.BlockIDMeta = reader.ReadVarInt();
                record.Coordinates = new Position(x, y, z);

                value[i] = record;
            }

            return value;
        }

        public void ToStream(IMinecraftStream stream)
        {
            stream.WriteVarInt(Count);

            foreach (var entry in _entries)
            {
            }
        }

        public IEnumerable<Record> GetRecords()
        {
            return _entries.ToArray();
        }
    }
}
