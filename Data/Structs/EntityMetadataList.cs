using System;
using System.Collections.Generic;
using System.Text;
using MineLib.Network.Data.EntityMetadata;
using MineLib.Network.IO;

namespace MineLib.Network.Data.Structs
{
    /// <summary>
    /// Used to send metadata with entities
    /// </summary>
    public class EntityMetadataList : IEquatable<EntityMetadataList>
    {
        private readonly Dictionary<byte, EntityMetadataEntry> _entries;

        public EntityMetadataList()
        {
            _entries = new Dictionary<byte, EntityMetadataEntry>();
        }

        public int Count
        {
            get { return _entries.Count; }
        }

        public EntityMetadataEntry this[byte index]
        {
            get { return _entries[index]; }
            set { _entries[index] = value; }
        }

        delegate EntityMetadataEntry CreateEntryInstance();

        private static readonly CreateEntryInstance[] EntryTypes =
        {
            () => new EntityMetadataByte(),           // 0
            () => new EntityMetadataShort(),          // 1
            () => new EntityMetadataInt(),            // 2
            () => new EntityMetadataFloat(),          // 3
            () => new EntityMetadataString(),         // 4
            () => new EntityMetadataSlot(),           // 5
            () => new EntityMetadataVector(),         // 6
            () => new EntityMetadataRotation()        // 7
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

        public static EntityMetadataList FromReader(IMinecraftDataReader reader)
        {
            var value = new EntityMetadataList();
            while (true)
            {
                byte key = reader.ReadByte();
                if (key == 127) break;

                var type = (byte)((key & 0xE0) >> 5);
                var index = (byte)(key & 0x1F);

                var entry = EntryTypes[type]();
                entry.FromReader(reader);
                entry.Index = index;

                value[index] = entry;
            }
            return value;
        }

        public void ToStream(IMinecraftStream stream)
        {
            foreach (var entry in _entries)
                entry.Value.ToStream(stream, entry.Key);

            stream.WriteByte(0x7F);
        }

        #endregion

        public bool Equals(EntityMetadataList other)
        {
            if (!Count.Equals(other.Count))
                return false;

            for (int i = 0; i < Count; i++)
                if (!this[(byte) i].Equals(other[(byte) i])) return false;
            
            return true;
        }

        public override bool Equals(object obj)
        {
            if (obj.GetType() != typeof(EntityMetadataList))
                return false;

            return Equals((EntityMetadataList) obj);
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
