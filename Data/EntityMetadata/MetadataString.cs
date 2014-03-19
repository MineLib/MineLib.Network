using System;
using MineLib.Network.IO;


namespace MineLib.Network.Data.EntityMetadata
{
    public class MetadataString : MetadataEntry
    {
        public string Value;

        public MetadataString()
        {
        }

        public MetadataString(string value)
        {
            if (value.Length > 16)
                throw new ArgumentOutOfRangeException("value", "Maximum string length is 16 characters");
            while (value.Length < 16)
                value = value + "\0";
            Value = value;
        }

        public override byte Identifier
        {
            get { return 4; }
        }

        public override string FriendlyName
        {
            get { return "string"; }
        }

        public static implicit operator MetadataString(string value)
        {
            return new MetadataString(value);
        }

        public override void FromStream(ref PacketByteReader stream)
        {
            Value = stream.ReadString();
        }

        public override void WriteTo(ref PacketStream stream, byte index)
        {
            stream.WriteByte(GetKey(index));
            stream.WriteString(Value);
        }
    }
}