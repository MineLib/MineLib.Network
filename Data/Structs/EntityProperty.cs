using System;
using System.Collections.Generic;
using MineLib.Network.IO;
using Org.BouncyCastle.Math;

namespace MineLib.Network.Data.Structs
{
    public struct Modifiers
    {
        public BigInteger UUID;
        public float Amount;
        public sbyte Operation;
    }

    public struct EntityProperty
    {
        public string Key;
        public float Value;
        public Modifiers[] Modifiers;
    }

    public class EntityPropertyList : IEquatable<EntityPropertyList>
    {
        private readonly List<EntityProperty> _entries;

        public EntityPropertyList()
        {
            _entries = new List<EntityProperty>();
        }

        public int Count
        {
            get { return _entries.Count; }
        }

        public EntityProperty this[int index]
        {
            get { return _entries[index]; }
            set { _entries.Insert(index, value); }
        }

        #region Network

        public static EntityPropertyList FromReader(IMinecraftDataReader reader)
        {
            var count = reader.ReadInt();

            var value = new EntityPropertyList();
            for (int i = 0; i < count; i++)
            {
                var property = new EntityProperty();

                property.Key = reader.ReadString();
                property.Value = (float) reader.ReadDouble();
                var listLength = reader.ReadVarInt();

                property.Modifiers = new Modifiers[listLength];
                for (var j = 0; j < listLength; j++)
                {
                    var item = new Modifiers
                    {
                        UUID = reader.ReadBigInteger(),
                        Amount = (float) reader.ReadDouble(),
                        Operation = reader.ReadSByte()
                    };

                    property.Modifiers[j] = item;
                }

                value[i] = property;
            }

            return value;
        }

        public void ToStream(ref IMinecraftStream stream)
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

        public bool Equals(EntityPropertyList other)
        {
            if (!Count.Equals(other.Count))
                return false;

            for (int i = 0; i < Count; i++)
                if (!this[i].Equals(other[i])) return false;
            
            return true;
        }

        public override bool Equals(object obj)
        {
            if (obj.GetType() != typeof(EntityPropertyList))
                return false;

            return Equals((EntityPropertyList) obj);
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
