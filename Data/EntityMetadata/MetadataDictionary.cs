using System.Collections.Generic;
using System.Text;
using MineLib.Network.IO;

namespace MineLib.Network.Data.EntityMetadata
{
    /// <summary>
    ///     Used to send metadata with entities
    /// </summary>
    public class MetadataDictionary
    {
        private readonly Dictionary<byte, MetadataEntry> _entries;

        public MetadataDictionary()
        {
            _entries = new Dictionary<byte, MetadataEntry>();
        }

        public int Count
        {
            get { return _entries.Count; }
        }

        public MetadataEntry this[byte index]
        {
            get { return _entries[index]; }
            set { _entries[index] = value; }
        }

        public static MetadataDictionary FromReader(PacketByteReader reader)
        {
            var value = new MetadataDictionary();
            while (true)
            {
                byte key = reader.ReadByte();
                if (key == 127) break;

                byte type = (byte)((key & 0xE0) >> 5);
                byte index = (byte)(key & 0x1F);

                var entry = EntryTypes[type]();
                entry.FromReader(reader);
                entry.Index = index;

                value[index] = entry;
            }
            return value;
        }

        public void WriteTo(ref PacketStream stream)
        {
            foreach (var entry in _entries)
                entry.Value.WriteTo(ref stream, entry.Key);
            stream.WriteByte(0x7F);
        }

        delegate MetadataEntry CreateEntryInstance();

        private static readonly CreateEntryInstance[] EntryTypes = new CreateEntryInstance[]
            {
                () => new MetadataByte(),   // 0
                () => new MetadataShort(),  // 1
                () => new MetadataInt(),    // 2
                () => new MetadataFloat(),  // 3
                () => new MetadataString(), // 4
                () => new MetadataSlot()    // 5
            };

        public override string ToString()
        {
            StringBuilder sb = null;

            foreach (var entry in _entries.Values)
            {
                if (sb != null)
                    sb.Append(", ");
                else
                    sb = new StringBuilder();

                sb.Append(entry);
            }

            if (sb != null)
                return sb.ToString();

            return string.Empty;
        }
    }
}
