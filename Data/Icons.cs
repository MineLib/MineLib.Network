using System.Collections.Generic;
using MineLib.Network.IO;

namespace MineLib.Network.Data
{
    public struct Icon
    {
        public byte Direction;
        public byte Type;
        public int X;
        public int Y;
    }

    public class Icons
    {
        private readonly List<Icon> _entries;

        public Icons()
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

        public static Icons FromReader(PacketByteReader reader)
        {
            var value = new Icons();

            var count = reader.ReadVarInt();
            for (var i = 0; i < count; i++)
            {
                var icon = new Icon();

                byte comb = reader.ReadByte();

                //icon.Direction = 
                //icon.Type = 

                icon.X = reader.ReadByte();
                icon.Y = reader.ReadByte();

                value[i] = icon;
            }

            return value;
        }

        public void ToStream(ref PacketStream stream)
        {
            stream.WriteVarInt(Count);

            foreach (var entry in _entries)
            {
                stream.WriteByte((byte)((byte)(entry.Direction << 4) | entry.Type));
                stream.WriteByte((byte)entry.X);
                stream.WriteByte((byte)entry.Y);
            }
        }
    }

}
