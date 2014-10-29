using System.Collections.Generic;
using MineLib.Network.IO;

namespace MineLib.Network.Modern.Data
{
    public struct ChunkColumnMetadata
    {
        public Coordinates2D Coordinates;
        public ushort PrimaryBitMap;

        // -- Debugging
        public int[] PrimaryBitMapConverted { get { return Converter.ConvertUShort(PrimaryBitMap); } }
        // -- Debugging
    }

    public class ChunkColumnMetadataList
    {
        private readonly List<ChunkColumnMetadata> _entries;

        public ChunkColumnMetadataList()
        {
            _entries = new List<ChunkColumnMetadata>();
        }

        public int Count
        {
            get { return _entries.Count; }
        }

        public ChunkColumnMetadata this[int index]
        {
            get { return _entries[index]; }
            set { _entries.Insert(index, value); }
        }

        public IEnumerable<ChunkColumnMetadata> GetMetadata()
        {
            return _entries.ToArray();
        }

        public static ChunkColumnMetadataList FromReader(IMinecraftDataReader reader)
        {
            var value = new ChunkColumnMetadataList();

            var count = reader.ReadVarInt();
            for (var i = 0; i < count; i++)
            {
                var metadata = new ChunkColumnMetadata();

                metadata.Coordinates.X = reader.ReadInt();
                metadata.Coordinates.Z = reader.ReadInt();
                metadata.PrimaryBitMap = reader.ReadUShort();

                value[i] = metadata;
            }

            return value;
        }

        public void ToStream(IMinecraftStream stream)
        {
            stream.WriteVarInt(Count);

            foreach (var entry in _entries)
            {
                stream.WriteInt(entry.Coordinates.X);
                stream.WriteInt(entry.Coordinates.Z);
                stream.WriteUShort(entry.PrimaryBitMap);
            }
        }
    }
}
