using System.Collections.Generic;
using MineLib.Network.IO;

namespace MineLib.Network.Data
{
    public struct Metadata
    {
        public Coordinates2D Coordinates;
        public ushort PrimaryBitMap;

        // -- Debugging
        public int[] PrimaryBitMapConverted { get { return Converter.ConvertUShort(PrimaryBitMap); } }
        // -- Debugging
    }

    public class ChunkColumnMetadata
    {
        private readonly List<Metadata> _entries;

        public ChunkColumnMetadata()
        {
            _entries = new List<Metadata>();
        }

        public int Count
        {
            get { return _entries.Count; }
        }

        public Metadata this[int index]
        {
            get { return _entries[index]; }
            set { _entries.Insert(index, value); }
        }

        public static ChunkColumnMetadata FromReader(PacketByteReader reader)
        {
            var value = new ChunkColumnMetadata();

            var count = reader.ReadVarInt();
            for (var i = 0; i < count; i++)
            {
                var metadata = new Metadata();

                metadata.Coordinates.X = reader.ReadInt();
                metadata.Coordinates.Z = reader.ReadInt();
                metadata.PrimaryBitMap = reader.ReadUShort();

                value[i] = metadata;
            }

            return value;
        }

        public void ToStream(ref PacketStream stream)
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
