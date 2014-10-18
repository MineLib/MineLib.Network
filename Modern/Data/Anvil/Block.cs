using System;

namespace MineLib.Network.Modern.Data.Anvil
{
    public struct Block : IEquatable<Block>
    {
        public readonly short IDMeta;   // 2 byte
        public byte SkyAndBlockLight;   // 1 byte
                                        // 3 byte

        public Block(short id)
        {
            IDMeta = (short) ((id & 0xFFF0) | 0);
            SkyAndBlockLight = 0;
        }

        public Block(short id, byte meta)
        {
            IDMeta = (short) ((id & 0xFFF0) | meta);
            SkyAndBlockLight = 0;
        }

        public Block(short id, byte meta, byte light)
        {
            IDMeta = (short) ((id & 0xFFF0) | meta);
            SkyAndBlockLight = (byte) ((0 & 0xF) | (light >> 0xF));
        }

        public Block(short id, byte meta, byte light, byte skyLight)
        {
            IDMeta = (short) ((id & 0xFFF0) | meta);
            SkyAndBlockLight = (byte) ((skyLight & 0xF) | (light >> 0xF));
        }

        public override string ToString()
        {
            return String.Format("ID: {0}, Meta: {1}", GetID(), GetMeta());
        }

        public short GetID()
        {
            return (short) (IDMeta >> 4);
        }

        public byte GetMeta()
        {
            return (byte) (IDMeta & 0xF);
        }

        public byte GetSkyLight()
        {
            return (byte) (SkyAndBlockLight >> 4);
        }

        public byte GetLight()
        {
            return (byte) (SkyAndBlockLight & 0xF);
        }

        public void SetSkyLight(byte skyLight)
        {
            var bLight = GetLight();
            SkyAndBlockLight = (byte) ((skyLight & 0xF) | (bLight >> 0xF));
        }

        public void SetLight(byte blockLight)
        {
            var skyLight = GetSkyLight();
            SkyAndBlockLight = (byte) ((skyLight & 0xF) | (blockLight >> 0xF));
        }


        public bool Equals(Block other)
        {
            return other.IDMeta.Equals(IDMeta);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (obj.GetType() != typeof (Block)) return false;
            return Equals((Block) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var result = IDMeta.GetHashCode();
                return result;
            }
        }
    }
}