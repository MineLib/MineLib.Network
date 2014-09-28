using System;

namespace MineLib.Network.Data.Anvil
{
    public struct Block : IEquatable<Block>
    {
        public readonly short ID;
        public readonly byte Meta;

        public byte Light;
        public byte SkyLight;
        
        public Block(short id)
        {
            ID = id;
            Meta = 0;

            Light = 0;
            SkyLight = 0;
        }

        public Block(short id, byte meta)
        {
            ID = id;
            Meta = meta;

            Light = 0;
            SkyLight = 0;
        }

        public Block(short id, byte meta, byte light)
        {
            ID = id;
            Meta = meta;

            Light = light;
            SkyLight = 0;
        }

        public Block(short id, byte meta, byte light, byte skyLight)
        {
            ID = id;
            Meta = meta;

            Light = light;
            SkyLight = skyLight;
        }
        
        public override string ToString()
        {
            return string.Format("ID: {0}, Meta: {1}", ID, Meta);
        }

        public bool Equals(Block other)
        {
            return other.ID.Equals(ID) && other.Meta.Equals(Meta);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (obj.GetType() != typeof(Block)) return false;
            return Equals((Block)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var result = ID.GetHashCode();
                result = (result * 397) ^ Meta.GetHashCode();
                return result;
            }
        }
    }
}