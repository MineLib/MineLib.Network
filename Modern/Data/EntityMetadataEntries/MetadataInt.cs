using MineLib.Network.IO;

namespace MineLib.Network.Modern.Data.EntityMetadataEntries
{
    /// <summary>
    /// Int32 Metadata
    /// </summary>
    public class MetadataInt : MetadataEntry
    {
        public override byte Identifier { get { return 2; } }
        public override string FriendlyName { get { return "int"; } }

        public int Value;

        public static implicit operator MetadataInt(int value)
        {
            return new MetadataInt(value);
        }

        public MetadataInt()
        {
        }

        public MetadataInt(int value)
        {
            Value = value;
        }

        public override void FromReader(IMinecraftDataReader reader)
        {
            Value = reader.ReadInt();
        }

        public override void ToStream(IMinecraftStream stream, byte index)
        {
            stream.WriteByte(GetKey(index));
            stream.WriteInt(Value);
        }
    }
}