using MineLib.Network.IO;

namespace MineLib.Network.Modern.Data.EntityMetadataEntries
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
            Rotation = new Rotation(0,0,0);
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

        public override void FromReader(MinecraftDataReader reader)
        {
            Rotation.Pitch = reader.ReadFloat();
            Rotation.Yaw = reader.ReadFloat();
            Rotation.Roll = reader.ReadFloat();
        }

        public override void ToStream(MinecraftStream stream, byte index)
        {
            stream.WriteByte(GetKey(index));
            stream.WriteFloat(Rotation.Pitch);
            stream.WriteFloat(Rotation.Yaw);
            stream.WriteFloat(Rotation.Roll);
        }
    }
}
