using System;
using System.Collections.Generic;
using MineLib.Network.IO;

namespace MineLib.Network.Data.Structs
{
    public struct Icon : IEquatable<Icon>
    {
        public byte Direction;
        public byte Type;
        public int X;
        public int Y;


        public static bool operator ==(Icon a, Icon b)
        {
            return a.Direction == b.Direction && a.Type == b.Type && a.X == b.X && a.Y == b.Y;
        }

        public static bool operator !=(Icon a, Icon b)
        {
            return a.Direction != b.Direction && a.Type != b.Type && a.X != b.X && a.Y != b.Y;
        }

        public bool Equals(Icon other)
        {
            return Direction.Equals(other.Direction) && Type.Equals(other.Type) && X.Equals(other.X) && Y.Equals(other.Y);
        }

        public override bool Equals(object obj)
        {
            if (obj.GetType() != typeof(Icon))
                return false;

            return Equals((Icon) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var result = Direction.GetHashCode();
                result = (result * 397) ^ Type.GetHashCode();
                result = (result * 397) ^ X.GetHashCode();
                result = (result * 397) ^ Y.GetHashCode();
                return result;
            }
        }
    }

    public class IconList
    {
        private readonly List<Icon> _entries;

        public IconList()
        {
            _entries = new List<Icon>();
        }

        public int Count
        {
            get { return _entries.Count; }
        }

        public Icon this[int index]
        {
            get { return _entries[index]; }
            set { _entries.Insert(index, value); }
        }

        public static IconList FromReader(IMinecraftDataReader reader)
        {
            var value = new IconList();

            var count = reader.ReadVarInt();
            for (int i = 0; i < count; i++)
            {
                var icon = new Icon();

                var comb = reader.ReadByte();
                icon.Direction = (byte)(comb & 0xF0);
                icon.Type = (byte)(comb & 0x0F);

                icon.X = reader.ReadByte();
                icon.Y = reader.ReadByte();

                value[i] = icon;
            }

            return value;
        }

        public void ToStream(IMinecraftStream stream)
        {
            stream.WriteVarInt(Count);

            foreach (var entry in _entries)
            {
                stream.WriteByte((byte) ((entry.Direction << 4) | entry.Type));
                stream.WriteByte((byte) entry.X);
                stream.WriteByte((byte) entry.Y);
            }
        }
    }

}
