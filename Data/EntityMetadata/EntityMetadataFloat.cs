using MineLib.Network.IO;

namespace MineLib.Network.Data.EntityMetadata
{
    /// <summary>
    /// Float Metadata
    /// </summary>
    public class EntityMetadataFloat : EntityMetadataEntry
    {
        public override byte Identifier { get { return 3; } }
        public override string FriendlyName { get { return "float"; } }

        public float Value;

        public static implicit operator EntityMetadataFloat(float value)
        {
            return new EntityMetadataFloat(value);
        }

        public EntityMetadataFloat()
        {
        }

        public EntityMetadataFloat(float value)
        {
            Value = value;
        }

        public override void FromReader(IMinecraftDataReader reader)
        {
            Value = reader.ReadFloat();
        }

        public override void ToStream(IMinecraftStream stream, byte index)
        {
            stream.WriteByte(GetKey(index));
            stream.WriteFloat(Value);
        }
    }
}
