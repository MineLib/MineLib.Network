using MineLib.Network.IO;

namespace MineLib.Network.Data.EntityMetadata
{
    /// <summary>
    /// Byte Metadata
    /// </summary>
    public class EntityMetadataByte : EntityMetadataEntry
    {
        public override byte Identifier { get { return 0; } }
        public override string FriendlyName { get { return "byte"; } }

        public byte Value;

        public static implicit operator EntityMetadataByte(byte value)
        {
            return new EntityMetadataByte(value);
        }

        public EntityMetadataByte()
        {
        }

        public EntityMetadataByte(byte value)
        {
            Value = value;
        }

        public override void FromReader(IMinecraftDataReader reader)
        {
            Value = reader.ReadByte();
        }

        public override void ToStream(IMinecraftStream stream, byte index)
        {
            stream.WriteByte(GetKey(index));
            stream.WriteByte(Value);
        }
    }
}
