using MineLib.Network.IO;

namespace MineLib.Network.Main.Data.EntityMetadataEntries
{
    /// <summary>
    /// Short Metadata
    /// </summary>
    public class MetadataShort : MetadataEntry
    {
        public override byte Identifier { get { return 1; } }
        public override string FriendlyName { get { return "short"; } }

        public short Value;

        public static implicit operator MetadataShort(short value)
        {
            return new MetadataShort(value);
        }

        public MetadataShort()
        {
        }

        public MetadataShort(short value)
        {
            Value = value;
        }

        public override void FromReader(PacketByteReader reader)
        {
            Value = reader.ReadShort();
        }

        public override void ToStream(ref PacketStream stream, byte index)
        {
            stream.WriteByte(GetKey(index));
            stream.WriteShort(Value);
        }
    }
}
