using CWrapped;

namespace MineLib.Network.Data.EntityMetadata
{
    public class MetadataFloat : MetadataEntry
    {
        public float Value;

        public MetadataFloat()
        {
        }

        public MetadataFloat(float value)
        {
            Value = value;
        }

        public override byte Identifier
        {
            get { return 3; }
        }

        public override string FriendlyName
        {
            get { return "float"; }
        }

        public static implicit operator MetadataFloat(float value)
        {
            return new MetadataFloat(value);
        }

        public override void FromStream(ref Wrapped stream)
        {
            Value = stream.ReadFloat();
        }

        public override void WriteTo(ref Wrapped stream, byte index)
        {
            stream.WriteByte(GetKey(index));
            stream.WriteFloat(Value);
        }
    }
}
