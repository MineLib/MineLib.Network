using MineLib.Network.IO;

namespace MineLib.Network.Data.EntityMetadata
{
    /// <summary>
    /// Int32 Metadata
    /// </summary>
    public class EntityMetadataInt : EntityMetadataEntry
    {
        public override byte Identifier { get { return 2; } }
        public override string FriendlyName { get { return "int"; } }

        public int Value;

        public static implicit operator EntityMetadataInt(int value)
        {
            return new EntityMetadataInt(value);
        }

        public EntityMetadataInt()
        {
        }

        public EntityMetadataInt(int value)
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