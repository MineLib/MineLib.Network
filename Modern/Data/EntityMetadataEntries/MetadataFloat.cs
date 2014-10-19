using MineLib.Network.IO;

namespace MineLib.Network.Modern.Data.EntityMetadataEntries
{
    /// <summary>
    /// Float Metadata
    /// </summary>
    public class MetadataFloat : MetadataEntry
    {
        public override byte Identifier { get { return 3; } }
        public override string FriendlyName { get { return "float"; } }

        public float Value;

        public static implicit operator MetadataFloat(float value)
        {
            return new MetadataFloat(value);
        }

        public MetadataFloat()
        {
        }

        public MetadataFloat(float value)
        {
            Value = value;
        }

        public override void FromReader(MinecraftDataReader reader)
        {
            Value = reader.ReadFloat();
        }

        public override void ToStream(MinecraftStream stream, byte index)
        {
            stream.WriteByte(GetKey(index));
            stream.WriteFloat(Value);
        }
    }
}
