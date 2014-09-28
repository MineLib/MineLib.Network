using System;
using System.Collections.Generic;

namespace MineLib.Network.Data.Anvil
{
    public class Section : IEquatable<Section>
    {
        public const int Width = 16;
        public const int Height = 16;
        public const int Depth = 16;

        public Block[] Blocks;

        public readonly short Y;

        public bool IsFilled;

        public Section(short y)
        {
            Y = y;
        }

        public override string ToString()
        {
            return IsFilled ? "Filled" : "Empty";
        }

        public void BuildEmpty()
        {
            if(IsFilled)
                return;

            Blocks = new Block[Width * Height * Depth];

            IsFilled = true;
        }

        public void BuildFromRawData(byte[] rawBlocks, byte[] rawBlockLight, byte[] rawSkyLight)
        {
            if(IsFilled) 
                return;

            Blocks = new Block[Width * Height * Depth];

            // TODO: Less messy
            var idMetadata = new short[4096];

            for (int i = 0, j = 0; i < idMetadata.Length; i++)
            {
                idMetadata[i] = (short)(rawBlocks[j] + rawBlocks[j + 1]);
                j++;
                j++;
            }

            var blockLight = ToBytePerBlock(rawBlockLight);
            var skyLight = ToBytePerBlock(rawSkyLight);

            // TODO: Add auto Coordinate calculator
            for (var i = 0; i < Blocks.Length; i++)
            {
                var id = (short) (idMetadata[i] >> 4);
                var meta = (byte)(idMetadata[i] & 15);

                Blocks[i] = new Block(id, meta, blockLight[i], skyLight[i]);
            }

            IsFilled = true;
        }

        public Block GetBlock(Position sectionCoordinates)
        {
            if (!IsFilled)
                throw new AccessViolationException("Section is empty");

            return Blocks[GetIndexInArray(sectionCoordinates)];
        }

        public void SetBlock(Position sectionCoordinates, Block block)
        {
            if (!IsFilled)
                BuildEmpty();

            var index = GetIndexInArray(sectionCoordinates);

            // I don't think that these values will change
            block.Light = Blocks[index].Light;
            block.SkyLight = Blocks[index].SkyLight;

            Blocks[index] = block;
        }

        public byte GetBlockLighting(Position sectionCoordinates)
        {
            if (!IsFilled)
                throw new AccessViolationException("Section is empty");

            return Blocks[GetIndexInArray(sectionCoordinates)].Light;
        }

        public void SetBlockLighting(Position sectionCoordinates, byte data)
        {
            if (!IsFilled)
                BuildEmpty();

            Blocks[GetIndexInArray(sectionCoordinates)].Light = data;
        }

        public byte GetBlockSkylight(Position sectionCoordinates)
        {
            if (!IsFilled)
                throw new AccessViolationException("Section is empty");

            return Blocks[GetIndexInArray(sectionCoordinates)].SkyLight;
        }

        public void SetBlockSkylight(Position sectionCoordinates, byte data)
        {
            if (!IsFilled)
                BuildEmpty();

            Blocks[GetIndexInArray(sectionCoordinates)].SkyLight = data;
        }

        #region Helping Methods

        private static int GetIndexInArray(Position sectionCoordinates)
        {
            return (sectionCoordinates.X + (sectionCoordinates.Z * 16) + (sectionCoordinates.Y * 16 * 16));
        }

        public static Position GetSectionPositionInArray(int index)
        {
            return new Position
            {
                X = index % 16,
                Y = index / (16 * 16),
                Z = (index / 16) % 16
            };
        }

        private static byte[] ToBytePerBlock(IList<byte> halfByteData)
        {
            var newMeta = new byte[Width * Height * Depth];

            if (halfByteData.Count != Width * Height * Depth / 2)
                throw new ArgumentOutOfRangeException("halfByteData", "Length != Half Byte Metadata length");

            for (var i = 0; i < halfByteData.Count; i++)
            {
                var block2 = (byte)((halfByteData[i] >> 4) & 15);
                var block1 = (byte)(halfByteData[i] & 15);

                newMeta[(i * 2)] = block1;
                newMeta[(i * 2) + 1] = block2;
            }

            return newMeta;
        }

        private static byte[] ToHalfBytePerBlock(IList<byte> byteData)
        {
            var newMeta = new byte[Width * Height * Depth / 2];

            if (byteData.Count != Width * Height * Depth)
                throw new ArgumentOutOfRangeException("byteData", "Length != Full Byte Metadata length");

            for (var i = 0; i < byteData.Count; i++)
            {
                // TODO: Convert Full Byte Metadata to Half Byte
            }

            return newMeta;
        }

        #endregion

        // You need to be a really freak to use it
        public bool Equals(Section section)
        {
            return Blocks.Equals(section.Blocks) && Y.Equals(section.Y);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (obj.GetType() != typeof(Section)) return false;
            return Equals((Section)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var result = Blocks.GetHashCode();
                result = (result * 397) ^ Y.GetHashCode();
                return result;
            }
        }
    }
}