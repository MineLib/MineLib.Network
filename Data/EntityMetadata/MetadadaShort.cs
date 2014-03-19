using MineLib.Network.IO;


namespace MineLib.Network.Data.EntityMetadata
{
    public class MetadataShort : MetadataEntry
    {
        public short Value;

        public MetadataShort()
        {
        }

        public MetadataShort(short value)
        {
            Value = value;
        }

        public override byte Identifier
        {
            get { return 1; }
        }

        public override string FriendlyName
        {
            get { return "short"; }
        }

        public static implicit operator MetadataShort(short value)
        {
            return new MetadataShort(value);
        }

        public override void FromStream(ref PacketByteReader stream)
        {
            Value = stream.ReadShort();
        }

        public override void WriteTo(ref PacketStream stream, byte index)
        {
            stream.WriteByte(GetKey(index));
            stream.WriteShort(Value);
        }
    }
}