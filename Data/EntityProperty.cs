using System;
using System.Collections.Generic;
using MineLib.Network.IO;
using Org.BouncyCastle.Math;

namespace MineLib.Network.Data
{
    public class Modifiers
    {
        public BigInteger UUID;
        public double Amount;
        public sbyte Operation;
    }

    public class Property
    {
        public string Key;
        public double Value;
        public Modifiers[] Modifiers;
    }

    public class EntityProperty : IEquatable<EntityProperty>
    {
        private readonly List<Property> _entries;

        public EntityProperty()
        {
            _entries = new List<Property>();
        }

        public int Count
        {
            get { return _entries.Count; }
        }

        public Property this[int index]
        {
            get { return _entries[index]; }
            set { _entries.Insert(index, value); }
        }

        #region Network

        public static EntityProperty FromReader(PacketByteReader reader)
        {
            var count = reader.ReadInt();

            var value = new EntityProperty();
            for (var i = 0; i < count; i++)
            {
                var property = new Property();

                property.Key = reader.ReadString();
                property.Value = reader.ReadDouble();
                var listLength = reader.ReadVarInt();

                property.Modifiers = new Modifiers[listLength];
                for (var j = 0; j < listLength; j++)
                {
                    var item = new Modifiers
                    {
                        UUID = reader.ReadBigInteger(),
                        Amount = reader.ReadDouble(),
                        Operation = reader.ReadSByte()
                    };

                    property.Modifiers[j] = item;
                }

                value[i] = property;
            }

            return value;
        }

        public void ToStream(ref PacketStream stream)
        {
            stream.WriteInt(Count);

            foreach (var entry in _entries)
            {
                stream.WriteString(entry.Key);
                stream.WriteDouble(entry.Value);

                stream.WriteShort((short)entry.Modifiers.Length);
                foreach (var modifiers in entry.Modifiers)
                {
                    stream.WriteBigInteger(modifiers.UUID);
                    stream.WriteDouble(modifiers.Amount);
                    stream.WriteSByte(modifiers.Operation);
                }
            }
        }

        #endregion

        public bool Equals(EntityProperty other)
        {
            if (other.Count != Count)
                return false;

            for (byte i = 0; i < (byte)Count; i++)
            {
                if (other[i] != this[i])
                    return false;
            }

            return true;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (obj.GetType() != typeof(EntityProperty)) return false;
            return Equals((EntityProperty)obj);
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
