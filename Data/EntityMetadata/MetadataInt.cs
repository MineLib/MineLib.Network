using CWrapped;

namespace MineLib.Network.Data.EntityMetadata
{
    public class MetadataInt : MetadataEntry
    {
        public int Value;

        public MetadataInt()
        {
        }

        public MetadataInt(int value)
        {
            Value = value;
        }

        public override byte Identifier
        {
            get { return 2; }
        }

        public override string FriendlyName
        {
            get { return "int"; }
        }

        public static implicit operator MetadataInt(int value)
        {
            return new MetadataInt(value);
        }

        public override void FromStream(ref Wrapped stream)
        {
            Value = stream.ReadInt();
        }

        public override void WriteTo(ref Wrapped stream, byte index)
        {
            stream.WriteByte(GetKey(index));
            stream.WriteInt(Value);
        }
    }
}
