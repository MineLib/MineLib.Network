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
        #region Nested type: CreateEntryInstance

        private delegate MetadataEntry CreateEntryInstance();

        #endregion

        private static readonly CreateEntryInstance[] EntryTypes = new CreateEntryInstance[]
                                                                       {
                                                                           () => new MetadataByte(),    // 0
                                                                           () => new MetadataShort(),   // 1
                                                                           () => new MetadataInt(),     // 2
                                                                           () => new MetadataFloat(),   // 3
                                                                           () => new MetadataString(),  // 4
                                                                           () => new MetadataSlot(),    // 5
                                                                       };

        private readonly Dictionary<byte, MetadataEntry> entries;

        public MetadataDictionary()
        {
            entries = new Dictionary<byte, MetadataEntry>();
        }

        public int Count
        {
            get { return entries.Count; }
        }

        public MetadataEntry this[byte index]
        {
            get { return entries[index]; }
            set { entries[index] = value; }
        }

        public static MetadataDictionary FromStream(ref PacketByteReader stream)
        {
            var value = new MetadataDictionary();
            while (true)
            {
                byte item = stream.ReadByte();
                if (item == 127) break;

                var index = (byte) (item & 31);
                var type = (byte) ((item & 224) >> 5);
                //var type = (byte)((item & 0xE0) >> 5);

                MetadataEntry entry = EntryTypes[type]();
                entry.FromStream(ref stream);
                entry.Index = index;

                value[index] = entry;
            }
            return value;
        }

        public void WriteTo(ref PacketStream stream)
        {
            foreach (var entry in entries)
                entry.Value.WriteTo(ref stream, entry.Key);
            stream.WriteByte(0x7F);
        }

        public override string ToString()
        {
            StringBuilder sb = null;

            foreach (MetadataEntry entry in entries.Values)
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