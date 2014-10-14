using System;
using System.Collections.Generic;

namespace MineLib.Network.Modern.Data.Anvil
{
    public struct Section : IEquatable<Section>
    {
        public const int Width = 16;
        public const int Height = 16;
        public const int Depth = 16;

        public Block[,,] Blocks;

        public bool IsFilled;

        public Position Position;

        public Section(Position position)
        {
            Position = position;

            Blocks = new Block[0, 0, 0];
            IsFilled = false;
        }

        public override string ToString()
        {
            return IsFilled ? "Filled" : "Empty";
        }

        public void BuildEmpty()
        {
            if(IsFilled)
                return;

            Blocks = new Block[Width, Height, Depth];

            IsFilled = true;
        }

        public void BuildFromRawData(byte[] rawBlocks, byte[] rawBlockLight, byte[] rawSkyLight)
        {
            if(IsFilled) 
                return;

            Blocks = new Block[Width, Height, Depth];

            var blockLight = ToBytePerBlock(rawBlockLight);
            var skyLight = ToBytePerBlock(rawSkyLight);

            for (int i = 0, j = 0; i < Width * Height * Depth; i++)
            {
                var idMetadata = (short)(rawBlocks[j] + rawBlocks[j + 1]);
                j++;
                j++;

                // TODO: Add auto Coordinate calculator
                var id = (short)(idMetadata >> 4);
                var meta = (byte)(idMetadata & 15);

                var sectionPos = GetSectionPositionByIndex(i);
                if(id != 0 && meta != 0)
                    Blocks[sectionPos.X, sectionPos.Y, sectionPos.Z] = new Block(id, meta, blockLight[i], skyLight[i]);
                else
                    Blocks[sectionPos.X, sectionPos.Y, sectionPos.Z] = null;
            }

            IsFilled = true;
        }

        public Block GetBlock(Position sectionPos)
        {
            if (!IsFilled)
                throw new AccessViolationException("Section is empty");

            return Blocks[sectionPos.X, sectionPos.Y, sectionPos.Z];
        }

        public void SetBlock(Position sectionPos, Block block)
        {
            if (!IsFilled)
                BuildEmpty();

            var oldBlock = Blocks[sectionPos.X, sectionPos.Y, sectionPos.Z];

            // I don't think that these values will change
            block.Light = oldBlock.Light;
            block.SkyLight = oldBlock.SkyLight;

            Blocks[sectionPos.X, sectionPos.Y, sectionPos.Z] = block;
        }

        public byte GetBlockLighting(Position sectionPos)
        {
            if (!IsFilled)
                throw new AccessViolationException("Section is empty");

            return Blocks[sectionPos.X, sectionPos.Y, sectionPos.Z].Light;
        }

        public void SetBlockLighting(Position sectionPos, byte data)
        {
            if (!IsFilled)
                BuildEmpty();

            Blocks[sectionPos.X, sectionPos.Y, sectionPos.Z].Light = data;
        }

        public byte GetBlockSkylight(Position sectionPos)
        {
            if (!IsFilled)
                throw new AccessViolationException("Section is empty");

            return Blocks[sectionPos.X, sectionPos.Y, sectionPos.Z].SkyLight;
        }

        public void SetBlockSkylight(Position sectionPos, byte data)
        {
            if (!IsFilled)
                BuildEmpty();

            Blocks[sectionPos.X, sectionPos.Y, sectionPos.Z].SkyLight = data;
        }

        #region Helping Methods

        public static Position GetSectionPositionByIndex(int index)
        {
            return new Position
            {
                X = index % 16,
                Y = index / (16 * 16),
                Z = (index / 16) % 16
            };
        }

        public Position GetGlobalPositionByArrayIndex(Position pos)
        {
            return GetGlobalPositionByArrayIndex(pos.X, pos.Y, pos.Z);
        }

        public Position GetGlobalPositionByArrayIndex(int index1, int index2, int index3)
        {
            return GetGlobalPositionByIndex(16 * index1 + 16 * index2 + 16 * index3);
        }

        public Position GetGlobalPositionByIndex(int index)
        {
            var sectionPos = GetSectionPositionByIndex(index);

            return new Position
            {
                X = Width * Position.X + sectionPos.Y,
                Y = Height * Position.Y + sectionPos.Y,
                Z = Depth * Position.Z + sectionPos.Z
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
            return Blocks.Equals(section.Blocks) && Position.Equals(section.Position);
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
                result = (result * 397) ^ Position.GetHashCode();
                return result;
            }
        }
    }
}
