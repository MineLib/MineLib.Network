using System;
using System.Collections.Generic;
using MineLib.Network.IO;

namespace MineLib.Network.Main.Data.Anvil
{
    public class Chunk : IEquatable<Chunk>
    {
        public const short Width = 16;
        public const short Height = 256;
        public const short Depth = 16;

        public const int OneByteData = Section.Width * Section.Depth * Section.Height;
        public const int HalfByteData = OneByteData / 2;
        public const int TwoByteData = OneByteData * 2;
        public const int BiomesLength = Width * Depth;

        public Coordinates2D Coordinates;
        public ushort PrimaryBitMap;
        public bool OverWorld;
        public bool GroundUp;

        public byte[] Biomes;

        public Section[] Sections;

        // -- Debugging
        public int[] PrimaryBitMapConverted { get { return SectionStatus(PrimaryBitMap); } }
        // -- Debugging
    
        public Chunk(Coordinates2D chunkCoordinates)
        {
            Coordinates = chunkCoordinates;
            Biomes = new byte[BiomesLength];

            Sections = new Section[16];
            for (var i = 0; i < Sections.Length; i++)
            {
                var pos = new Position(Coordinates.X, i, Coordinates.Z);
                Sections[i] = new Section(pos);
            }
        }

        public override string ToString()
        {
            return string.Format("Filled Sections: {0}", GetFilledSectionsCount());
        }

        public static int GetSectionCount(ushort bitMap)
        {
            // Get the total sections included in the bitMap
            var sectionCount = 0;

            for (var y = 0; y < 16; y++)
            {
                if ((bitMap & (1 << y)) > 0)
                    sectionCount++;
            }

            return sectionCount;
        }

        #region Network

        public static Chunk FromReader(PacketByteReader reader)
        {
            var overWorld = true;// TODO: From World class
            var coordinates = new Coordinates2D(reader.ReadInt(), reader.ReadInt());
            var groundUp = reader.ReadBoolean();
            var primaryBitMap = reader.ReadUShort();
            
            var value = new Chunk(coordinates);
            value.OverWorld = overWorld;
            value.GroundUp = groundUp;
            value.PrimaryBitMap = primaryBitMap;

            var size = reader.ReadVarInt();
            var data = reader.ReadByteArray(size);

            var sectionCount = GetSectionCount(value.PrimaryBitMap);

            var chunkRawBlocks = new byte[sectionCount * TwoByteData];
            var chunkRawBlocksLight = new byte[sectionCount * HalfByteData];
            var chunkRawSkylight = new byte[sectionCount * HalfByteData];

            Array.Copy(data, 0, chunkRawBlocks, 0, chunkRawBlocks.Length);
            Array.Copy(data, chunkRawBlocks.Length, chunkRawBlocksLight, 0, chunkRawBlocksLight.Length);
            Array.Copy(data, chunkRawBlocks.Length + chunkRawBlocksLight.Length, chunkRawSkylight, 0, chunkRawSkylight.Length);

            for (int y = 0, i = 0; y < 16; y++)
            {
                if ((value.PrimaryBitMap & (1 << y)) > 0)
                {
                    // Blocks & Metadata
                    var rawBlocks = new byte[TwoByteData];
                    Array.Copy(chunkRawBlocks, i * rawBlocks.Length, rawBlocks, 0, rawBlocks.Length);

                    // Light, convert to 1 byte per block
                    var rawBlockLight = new byte[HalfByteData];
                    Array.Copy(chunkRawSkylight, i * rawBlockLight.Length, rawBlockLight, 0, rawBlockLight.Length);

                    // Sky light, convert to 1 byte per block
                    var rawSkyLight = new byte[HalfByteData];
                    if (value.OverWorld)
                        Array.Copy(chunkRawSkylight, i * rawSkyLight.Length, rawSkyLight, 0, rawSkyLight.Length);

                    value.Sections[i].BuildFromRawData(rawBlocks, rawBlockLight, rawSkyLight);
                    i++;
                }
            }
            if (value.GroundUp)
                Array.Copy(data, data.Length - value.Biomes.Length, value.Biomes, 0, value.Biomes.Length);

            return value;
        }

        // TODO: Convert to RawData
        public void ToStream(ref PacketStream stream)
        {
            var OverWorld = true; // TODO: From World class

            stream.WriteVarInt(Coordinates.X);
            stream.WriteVarInt(Coordinates.Z);
            stream.WriteBoolean(OverWorld);
            stream.WriteUShort(PrimaryBitMap);

            var sectionCount = GetSectionCount(PrimaryBitMap);

            var chunkRawBlocks = new byte[sectionCount * TwoByteData];
            var chunkRawBlocksLight = new byte[sectionCount * HalfByteData];
            var chunkRawSkylight = new byte[sectionCount * HalfByteData];

            var size = sectionCount * (TwoByteData + HalfByteData + (OverWorld ? HalfByteData : 0)) + (OverWorld ? Biomes.Length : 0);
            var data = new byte[size];

            for (int y = 0, i = 0; y < Sections.Length; y++)
            {
                var section = Sections[y];

                if (Sections[i].IsFilled)
                {
                    // Blocks & Metadata
                    //Array.Copy(section.RawBlocks, 0, chunkRawBlocks, i * section.RawBlocks.Length, section.RawBlocks.Length);
                    // Light
                    //Array.Copy(section.RawBlockLight, 0, chunkRawBlocksLight, i * section.RawBlockLight.Length, section.RawBlockLight.Length);
                    // Sky light
                    //if (OverWorld)
                    //    Array.Copy(section.RawSkyLight, 0, chunkRawSkylight, i  *section.RawSkyLight.Length, section.RawSkyLight.Length);
                    
                    i++;
                }
            }

            Array.Copy(chunkRawBlocks, 0, data, 0, chunkRawBlocks.Length);
            Array.Copy(chunkRawBlocksLight, 0, data, chunkRawBlocks.Length, chunkRawBlocksLight.Length);
            Array.Copy(chunkRawSkylight, 0, data, chunkRawBlocks.Length + chunkRawBlocksLight.Length, chunkRawSkylight.Length);
            if (OverWorld)
                Array.Copy(Biomes, 0, data, data.Length - Biomes.Length, Biomes.Length);

            stream.WriteVarInt(size);
            stream.WriteByteArray(data);
        }

        #endregion

        public Position GetBlockPosition(Section section, int index)
        {
            var sectionPosition = Section.GetSectionPositionByIndex(index);

            return new Position
            {
                X = 16 * Coordinates.X + sectionPosition.X,
                Y = 16 * section.Position.Y + sectionPosition.Y,
                Z = 16 * Coordinates.Z + sectionPosition.Z
            };
        }

        public Block GetBlock(Position coordinates)
        {
            var destSection = GetSectionByY(coordinates.Y);

            return destSection.GetBlock(GetSectionCoordinates(coordinates));
        }

        public void SetBlock(Position worldCoordinates, Block block)
        {
            var destSection = GetSectionByY(worldCoordinates.Y);

            destSection.SetBlock(GetSectionCoordinates(worldCoordinates), block);
        }

        public void SetBlockMultiBlock(Position coordinates, Block block)
        {
            var destSection = GetSectionByY(coordinates.Y);

            var coords = coordinates;
            coords.Y = GetYinSection(coordinates.Y);

            destSection.SetBlock(coords, block);
        }

        public byte GetBlockLight(Position worldCoordinates)
        {
            var destSection = GetSectionByY(worldCoordinates.Y);

            return destSection.GetBlockLighting(GetSectionCoordinates(worldCoordinates));
        }

        public void SetBlockLight(Position worldCoordinates, byte light)
        {
            var destSection = GetSectionByY(worldCoordinates.Y);

            destSection.SetBlockLighting(GetSectionCoordinates(worldCoordinates), light);
        }

        public byte GetBlockSkylight(Position worldCoordinates)
        {
            var destSection = GetSectionByY(worldCoordinates.Y);

            return destSection.GetBlockSkylight(GetSectionCoordinates(worldCoordinates));
        }

        public void SetBlockSkylight(Position worldCoordinates, byte light)
        {
            var destSection = GetSectionByY(worldCoordinates.Y);

            destSection.SetBlockSkylight(GetSectionCoordinates(worldCoordinates), light);
        }

        public byte GetBlockBiome(Position worldCoordinates)
        {
            var chunkCoordinates = GetChunkCoordinates(worldCoordinates);

            return Biomes[(chunkCoordinates.Z * 16) + chunkCoordinates.X];
        }

        public void SetBlockBiome(Position worldCoordinates, byte biome)
        {
            var chunkCoordinates = GetChunkCoordinates(worldCoordinates);

            Biomes[(chunkCoordinates.Z * 16) + chunkCoordinates.X] = biome;
        }

        public byte GetBlockBiome(Coordinates2D chunkCoordinates)
        {
            return Biomes[(chunkCoordinates.Z * 16) + chunkCoordinates.X];
        }

        public void SetBlockBiome(Coordinates2D chunkCoordinates, byte biome)
        {
            Biomes[(chunkCoordinates.Z * 16) + chunkCoordinates.X] = biome;
        }

        #region Helping Methods

        private Section GetSectionByY(int blockY)
        {
            return Sections[(byte)(blockY / 16)];
        }

        public static Position GetSectionCoordinates(Position coordinates, Coordinates2D chunkCoordinates)
        {
            return new Position
            {
                X = Math.Abs(coordinates.X - (chunkCoordinates.X * 16)),
                Y = coordinates.Y % 16,
                Z = coordinates.Z % chunkCoordinates.Z
            };
        }

        private Position GetSectionCoordinates(Position coordinates)
        {
            var chunk = GetChunkCoordinates(coordinates);

            // -- https://github.com/Azzi777/Umbra-Voxel-Engine/blob/master/Umbra%20Voxel%20Engine/Implementations/ChunkManager.cs#L172
            if (chunk.X != Coordinates.X || chunk.Z != Coordinates.Z)
                throw new ArgumentOutOfRangeException("coordinates","You stupid asshole!");

            return new Position
            {
                X = GetXinSection(coordinates.X),
                Y = GetYinSection(coordinates.Y),
                Z = GetZinSection(coordinates.Z)
            };
        }

        private static Coordinates2D GetChunkCoordinates(Position worldCoordinates)
        {
            return new Coordinates2D
            {
                X = worldCoordinates.X >> 4,
                Z = worldCoordinates.Z >> 4
            };
        }

        private int GetXinSection(int blockX)
        {
            return Math.Abs(blockX - (Coordinates.X * 16));
        }
        private int GetYinSection(int blockY)
        {
            return blockY % 16;
        }
        private int GetZinSection(int blockZ)
        {
            return blockZ % Coordinates.Z;
        }

        private int GetFilledSectionsCount()
        {
            var count = 0;
            foreach (var section in Sections)
            {
                if (section.IsFilled)
                    count++;
            }
            return count;
        }

        private int[] SectionStatus(ushort primaryBitMap)
        {
            return Converter.ConvertUShort(primaryBitMap);
        }

        #endregion

        // You need to be a really freak to use it
        public bool Equals(Chunk chunk)
        {
            return Coordinates.Equals(chunk.Coordinates) && Sections.Equals(chunk.Sections) && Biomes.Equals(chunk.Biomes);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (obj.GetType() != typeof(Chunk)) return false;
            return Equals((Chunk)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var result = Coordinates.GetHashCode();
                result = (result * 397) ^ Sections.GetHashCode();
                result = (result * 397) ^ Biomes.GetHashCode();
                return result;
            }
        }
    }

    public class ChunkList
    {
        private readonly List<Chunk> _entries;

        public ChunkColumnMetadataList Metadata;

        public bool GroundUp;

        public ChunkList()
        {
            _entries = new List<Chunk>();
        }

        public int Count
        {
            get { return _entries.Count; }
        }

        public Chunk this[int index]
        {
            get { return _entries[index]; }
            set { _entries.Insert(index, value); }
        }

        public IEnumerable<Chunk> GetChunk()
        {
            return _entries.ToArray();
        }

        public static ChunkList FromReader(PacketByteReader reader)
        {
            var value = new ChunkList();

            value.GroundUp = reader.ReadBoolean();
            value.Metadata = ChunkColumnMetadataList.FromReader(reader);

            int totalSections = 0;
            foreach (var metadata in value.Metadata.GetMetadata())
                totalSections += Chunk.GetSectionCount(metadata.PrimaryBitMap);


            var size = totalSections * (Chunk.TwoByteData + Chunk.HalfByteData + (value.GroundUp ? Chunk.HalfByteData : 0)) + value.Metadata.Count * Chunk.BiomesLength;
            var data = reader.ReadByteArray(size);

            int offset = 0;
            foreach (var metadata in value.Metadata.GetMetadata())
            {
                var chunk = new Chunk(metadata.Coordinates);
                chunk.PrimaryBitMap = metadata.PrimaryBitMap;
                chunk.OverWorld = true;

                var sectionCount = Chunk.GetSectionCount(chunk.PrimaryBitMap);

                var chunkRawBlocks = new byte[sectionCount * Chunk.TwoByteData];
                var chunkRawBlocksLight = new byte[sectionCount * Chunk.HalfByteData];
                var chunkRawSkylight = new byte[sectionCount * Chunk.HalfByteData];

                var chunkLength = sectionCount * (Chunk.TwoByteData + Chunk.HalfByteData + (chunk.OverWorld ? Chunk.HalfByteData : 0)) + Chunk.BiomesLength;
                var chunkData = new byte[chunkLength];
                Array.Copy(data, offset, chunkData, 0, chunkData.Length);

                Array.Copy(chunkData, 0, chunkRawBlocks, 0, chunkRawBlocks.Length);
                Array.Copy(chunkData, chunkRawBlocks.Length, chunkRawBlocksLight, 0, chunkRawBlocksLight.Length);
                Array.Copy(chunkData, chunkRawBlocks.Length + chunkRawBlocksLight.Length, chunkRawSkylight, 0, chunkRawSkylight.Length);
                if (value.GroundUp)
                    Array.Copy(chunkData, chunkRawBlocks.Length + chunkRawBlocksLight.Length + chunkRawSkylight.Length, chunk.Biomes, 0, Chunk.BiomesLength);

                for (int y = 0, i = 0; y < 16; y++)
                {
                    if ((chunk.PrimaryBitMap & (1 << y)) > 0)
                    {
                        // Blocks & Metadata
                        var rawBlocks = new byte[Chunk.TwoByteData];
                        Array.Copy(chunkRawBlocks, i * rawBlocks.Length, rawBlocks, 0, rawBlocks.Length);

                        // Light
                        var rawBlockLight = new byte[Chunk.HalfByteData];
                        Array.Copy(chunkRawSkylight, i * rawBlockLight.Length, rawBlockLight, 0, rawBlockLight.Length);

                        // Sky light
                        var rawSkyLight = new byte[Chunk.HalfByteData];
                        if (chunk.OverWorld)
                            Array.Copy(chunkRawSkylight, i * rawSkyLight.Length, rawSkyLight, 0, rawSkyLight.Length);

                        chunk.Sections[i].BuildFromRawData(rawBlocks, rawBlockLight, rawSkyLight);
                        i++;
                    }
                }
                value._entries.Add(chunk);

                offset += chunkLength;
            }

            return value;
        }

        // TODO: Convert to RawData
        public void ToStream(ref PacketStream stream)
        {
            stream.WriteBoolean(GroundUp);
            Metadata.ToStream(ref stream);

            int totalSections = 0;
            foreach (var metadata in Metadata.GetMetadata())
                totalSections += Chunk.GetSectionCount(metadata.PrimaryBitMap);

            var size = totalSections * (Chunk.TwoByteData + Chunk.HalfByteData + (GroundUp ? Chunk.HalfByteData : 0)) + Metadata.Count * 256;
            var data = new byte[size];

            int i = 0;
            int offset = 0;
            foreach (var metadata in _entries)
            {
                var sectionCount = Chunk.GetSectionCount(metadata.PrimaryBitMap);

                var chunkRawBlocks = new byte[sectionCount * Chunk.TwoByteData];
                var chunkRawBlocksLight = new byte[sectionCount * Chunk.HalfByteData];
                var chunkRawSkylight = new byte[sectionCount * Chunk.HalfByteData];

                var chunkLength = sectionCount * (Chunk.TwoByteData + Chunk.HalfByteData + (_entries[i].OverWorld ? Chunk.HalfByteData : 0)) + 256;
                var chunkData = new byte[chunkLength];

                int j = 0;
                foreach (var section in _entries[i].Sections)
                {
                    if (_entries[i].Sections[j].IsFilled)
                    {
                        // Blocks & Metadata
                        //Array.Copy(section.RawBlocks, 0, chunkRawBlocks, j * section.RawBlocks.Length, section.RawBlocks.Length);
                        // Light
                        //Array.Copy(section.RawBlockLight, 0, chunkRawBlocksLight, j * section.RawBlockLight.Length, section.RawBlockLight.Length);
                        // Sky light
                        //if (_entries[i].OverWorld)
                        //    Array.Copy(section.RawSkyLight, 0, chunkRawSkylight, j * section.RawSkyLight.Length, section.RawSkyLight.Length);

                        j++;
                    }
                }

                Array.Copy(chunkRawBlocks, 0, chunkData, 0, chunkRawBlocks.Length);
                Array.Copy(chunkRawBlocksLight, 0, chunkData, chunkRawBlocks.Length, chunkRawBlocksLight.Length);
                Array.Copy(chunkRawSkylight, 0, chunkData, chunkRawBlocks.Length + chunkRawBlocksLight.Length, chunkRawSkylight.Length);
                if (_entries[i].OverWorld)
                    Array.Copy(_entries[i].Biomes, 0, chunkData, chunkData.Length - _entries[i].Biomes.Length, _entries[i].Biomes.Length);

                Array.Copy(chunkData, 0, data, offset, chunkData.Length);


                offset += chunkLength;
                i++;
            }

            stream.WriteVarInt(size);
            stream.WriteByteArray(data);
        }
    }
}
