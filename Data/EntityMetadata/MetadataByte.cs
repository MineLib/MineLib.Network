using MineLib.Network.IO;


namespace MineLib.Network.Data.EntityMetadata
{
    public class MetadataByte : MetadataEntry
    {
        public byte Value;

        public MetadataByte()
        {
        }

        public MetadataByte(byte value)
        {
            Value = value;
        }

        public override byte Identifier
        {
            get { return 0; }
        }

        public override string FriendlyName
        {
            get { return "byte"; }
        }

        public static implicit operator MetadataByte(byte value)
        {
            return new MetadataByte(value);
        }

        public override void FromStream(ref PacketByteReader stream)
        {
            Value = stream.ReadByte();
        }

        public override void WriteTo(ref PacketStream stream, byte index)
        {
            stream.WriteByte(GetKey(index));
            stream.WriteByte(Value);
        }
    }
}