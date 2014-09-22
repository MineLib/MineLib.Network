using System;
using System.Collections.Generic;
using MineLib.Network.IO;

namespace MineLib.Network.Data
{
    public struct Record
    {
        public int BlockID;
        public Position Coordinates;
    }

    // TODO: Records bitmask
    public class RecordsArray
    {
        private readonly List<Record> _entries;

        public RecordsArray()
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

        public static RecordsArray FromReader(PacketByteReader reader)
        {
            var value = new RecordsArray();

            var count = reader.ReadVarInt();
            var data = reader.ReadByteArray(count);

            for (var i = 0; i < count; i++)
            {
                var record = new Record();

                var blockData = new byte[4];
                Buffer.BlockCopy(data, (i * 4), blockData, 0, 4);

                record.BlockID = (blockData[2] << 4) | ((blockData[3] & 0xF0) >> 4);
                record.Coordinates.Y = (blockData[1]);
                record.Coordinates.Z = (blockData[0] & 0x0f);
                record.Coordinates.X = (blockData[0] >> 4) & 0x0f;
                //record.Metadata = blockData[3] & 0xF;

                value[i] = record;
            }

            return value;
        }

        public void ToStream(ref PacketStream stream)
        {
            stream.WriteVarInt(Count);

            foreach (var entry in _entries)
            {
            }
        }
    }
}
