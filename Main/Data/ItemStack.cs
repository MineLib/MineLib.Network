//using fNbt;
//using fNbt.Serialization;

using System;
using System.Collections.Generic;
using MineLib.Network.IO;

namespace MineLib.Network.Main.Data
{
    // TODO: Uses more memory than class
    public struct ItemStack  : ICloneable, IEquatable<ItemStack>
    {
        public short ID;
        public sbyte Count;
        public short Damage; // Level
        public byte[] NBTData;

        public ItemStack(short id)
        {
            ID = id;
            Damage = 0;
            Count = 1;
            NBTData = null;
        }

        public ItemStack(short id, sbyte count) : this(id)
        {
            Count = count;
        }

        public ItemStack(short id, sbyte count, short damage) : this(id, count)
        {
            Damage = damage;
        }

        public ItemStack(short id, sbyte count, short damage, byte[] nbtData) : this(id, count, damage)
        {
            NBTData = nbtData;

            if (Count == 0)
            {
                ID = -1;
                Damage = 0;
                NBTData = null;
            }
        }

        public override string ToString()
        {
            if (Empty)
                return "(Empty)";
            var result = "ID: " + ID;
            if (Count != 1) result += "; Count: " + Count;
            //if (Metadata != 0) result += "; Metadata: " + Metadata;
            if (NBTData != null) result += Environment.NewLine + NBTData;
            return "(" + result + ")";
        }

        #region NBT

        public static ItemStack FromNbt(byte[] compound)
        {
            //var itemStack = EmptyStack;
            //itemStack.ID = compound.Get<NbtShort>("id").Value;
            //itemStack.Damage = compound.Get<NbtShort>("Damage").Value;
            //itemStack.Count = (sbyte)compound.Get<NbtByte>("Count").Value;
            ////s.Index = compound.Get<NbtByte>("Slot").Value;
            //if (compound.Get<NbtCompound>("tag") != null)
            //    itemStack.NBTData = compound.Get<NbtCompound>("tag");
            //return itemStack;
            return EmptyStack;
        }

        public byte[] ToNbt()
        {
            //var nbtCompound = new NbtCompound();
            //nbtCompound.Add(new NbtShort("id", ID));
            //nbtCompound.Add(new NbtShort("Damage", Damage));
            //nbtCompound.Add(new NbtByte("Count", (byte)Count));
            ////c.Add(new NbtByte("Slot", (byte)Index));
            //if (NBTData != null)
            //    nbtCompound.Add(new NbtCompound("tag"));
            //return nbtCompound;
            return null;
        }

        #endregion

        #region Network

        public static ItemStack FromReader(PacketByteReader reader)
        {
            var itemStack = new ItemStack();

            itemStack.ID = reader.ReadShort();

            if (itemStack.ID == -1 || itemStack.Count == -1)
                return EmptyStack;
            

            itemStack.Count = reader.ReadSByte();
            itemStack.Damage = reader.ReadShort();

            var length = reader.ReadVarInt();
            if (length == -1 || length == 0)
                return itemStack;

            itemStack.NBTData = reader.ReadByteArray(length);
            // TODO: NBTTag reading

            return itemStack;
        }

        public void ToStream(ref PacketStream stream)
        {
            stream.WriteShort(ID);
            stream.WriteShort(Damage);
            stream.WriteShort(Count);

            //if (Empty)
            //    stream.WriteSByte(Slot);

            if (NBTData == null)
            {
                stream.WriteShort(-1);
                return;
            }
        }

        #endregion

        #region Operators

        public static bool operator ==(ItemStack left, ItemStack right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(ItemStack left, ItemStack right)
        {
            return !left.Equals(right);
        }

        #endregion

        public bool Empty
        {
            get { return ID == -1; }
        }
        
        public static ItemStack EmptyStack
        {
            get
            {
                return new ItemStack(-1);
            }
        }

        public object Clone()
        {
            return new ItemStack(ID, Count, Damage, NBTData);
        }

        public bool CanMerge(ItemStack other)
        {
            if (Empty || other.Empty)
                return true;
            return ID == other.ID && Damage == other.Damage && Equals(NBTData, other.NBTData);
        }
        
        public bool Equals(ItemStack other)
        {
            return ID == other.ID && Damage == other.Damage && Count == other.Count && Equals(NBTData, other.NBTData);
            //return ID == other.ID && Damage == other.Damage && Count == other.Count && Slot == other.Slot && Equals(NBTData, other.NBTData);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (obj.GetType() != typeof(ItemStack)) return false;
            return Equals((ItemStack)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = ID.GetHashCode();
                hashCode = (hashCode * 397) ^ Damage.GetHashCode();
                hashCode = (hashCode * 397) ^ Count.GetHashCode();
                //hashCode = (hashCode * 397) ^ Slot.GetHashCode();
                hashCode = (hashCode * 397) ^ (NBTData != null ? NBTData.GetHashCode() : 0);
                return hashCode;
            }
        }
    }

    public class ItemStackList : IEquatable<ItemStackList>
    {
        private readonly List<ItemStack> _entries;

        public ItemStackList()
        {
            _entries = new List<ItemStack>();
        }

        public int Count
        {
            get { return _entries.Count; }
        }

        public ItemStack this[int index]
        {
            get { return _entries[index]; }
            set { _entries.Insert(index, value); }
        }

        #region Network

        public static ItemStackList FromReader(PacketByteReader reader)
        {
            var value = new ItemStackList();

            var count = reader.ReadShort();
            for (int i = 0; i < count; i++)
            {
                var slot = new ItemStack();

                slot.ID = reader.ReadShort();

                if (slot.ID == -1 || slot.Count == -1)
                {
                    value[i] = ItemStack.EmptyStack;
                    break;
                }

                slot.Count = reader.ReadSByte();
                slot.Damage = reader.ReadShort();

                var length = reader.ReadVarInt();
                if (length == -1)
                {
                    value[i] = slot;
                    break;
                }

                slot.NBTData = reader.ReadByteArray(length);
            }

            return value;
        }

        public void ToStream(ref PacketStream stream)
        {
            foreach (var itemStack in _entries)
            {
                //if (itemStack.ID == 1) // AIR
                //    return;

                stream.WriteShort(itemStack.ID);
                stream.WriteShort(itemStack.Damage);
                stream.WriteShort(itemStack.Count);

                //if (itemStack.Empty)
                //    stream.WriteSByte(itemStack.Slot);

                if (itemStack.NBTData == null)
                {
                    stream.WriteShort(-1);
                    return;
                }
            }
        }

        #endregion

        #region Operators

        public static bool operator ==(ItemStackList left, ItemStackList right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(ItemStackList left, ItemStackList right)
        {
            return !left.Equals(right);
        }

        #endregion

        public bool Equals(ItemStackList other)
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
            if (obj.GetType() != typeof(ItemStackList)) return false;
            return Equals((ItemStackList)obj);
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
