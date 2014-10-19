using System;
using System.Collections.Generic;
using System.Text;
using MineLib.Network.IO;
using MineLib.Network.Modern.Data.EntityMetadataEntries;

namespace MineLib.Network.Modern.Data
{
    /// <summary>
    /// Used to send metadata with entities
    /// </summary>
    public class EntityMetadata : IEquatable<EntityMetadata>
    {
        private readonly Dictionary<byte, MetadataEntry> _entries;

        public EntityMetadata()
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

        delegate MetadataEntry CreateEntryInstance();

        private static readonly CreateEntryInstance[] EntryTypes =
        {
            () => new MetadataByte(),           // 0
            () => new MetadataShort(),          // 1
            () => new MetadataInt(),            // 2
            () => new MetadataFloat(),          // 3
            () => new MetadataString(),         // 4
            () => new MetadataSlot(),           // 5
            () => new MetadataVector(),         // 6
            () => new MetadataRotation()        // 7
        };

        /// <summary>
        /// Converts this EntityMetadata to a string.
        /// </summary>
        /// <returns></returns>
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

        #region Network

        public static EntityMetadata FromReader(MinecraftDataReader reader)
        {
            var value = new EntityMetadata();
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

        public void ToStream(MinecraftStream stream)
        {
            foreach (var entry in _entries)
                entry.Value.ToStream(stream, entry.Key);
            stream.WriteByte(0x7F);
        }

        #endregion

        public bool Equals(EntityMetadata other)
        {
            if(other.Count != Count)
                return false;

            for (byte i = 0; i < (byte)Count; i++)
            {
                if (other[i] != this[i])
                    return false;
            }

            return true;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (obj.GetType() != typeof(EntityMetadata)) return false;
            return Equals((EntityMetadata)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var result = _entries.GetHashCode();
                result = (result * 397) ^ Count.GetHashCode();
                return result;
            }
        }
    }
}
