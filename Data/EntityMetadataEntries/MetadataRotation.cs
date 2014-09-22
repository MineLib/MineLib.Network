using MineLib.Network.IO;

namespace MineLib.Network.Data.EntityMetadataEntries
{
    /// <summary>
    /// Rotation Metadata
    /// </summary>
    public class MetadataRotation : MetadataEntry
    {
        public override byte Identifier { get { return 7; } }
        public override string FriendlyName { get { return "rotation"; } }

        public Rotation Rotation;

        public MetadataRotation()
        {
        }

        public MetadataRotation(float pitch, float yaw, int roll)
        {
            Rotation.Pitch = pitch;
            Rotation.Yaw = yaw;
            Rotation.Roll = roll;
        }

        public MetadataRotation(Rotation rotation)
        {
            Rotation = rotation;
        }

        public override void FromReader(PacketByteReader reader)
        {
            Rotation.Pitch = reader.ReadFloat();
            Rotation.Yaw = reader.ReadFloat();
            Rotation.Roll = reader.ReadFloat();
        }

        public override void ToStream(ref PacketStream stream, byte index)
        {
            stream.WriteByte(GetKey(index));
            stream.WriteFloat(Rotation.Pitch);
            stream.WriteFloat(Rotation.Yaw);
            stream.WriteFloat(Rotation.Roll);
        }
    }
}
