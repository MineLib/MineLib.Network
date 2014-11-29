using MineLib.Network.IO;

namespace MineLib.Network.Data.EntityMetadata
{
    /// <summary>
    /// Rotation Metadata
    /// </summary>
    public class EntityMetadataRotation : EntityMetadataEntry
    {
        public override byte Identifier { get { return 7; } }
        public override string FriendlyName { get { return "rotation"; } }

        public Rotation Rotation;

        public EntityMetadataRotation()
        {
            Rotation = new Rotation(0,0,0);
        }

        public EntityMetadataRotation(float pitch, float yaw, float roll)
        {
            Rotation = new Rotation(pitch, yaw, roll);
        }

        public EntityMetadataRotation(Rotation rotation)
        {
            Rotation = rotation;
        }

        public override void FromReader(IMinecraftDataReader reader)
        {
            Rotation = new Rotation(
                reader.ReadFloat(), 
                reader.ReadFloat(), 
                reader.ReadFloat());
        }

        public override void ToStream(IMinecraftStream stream, byte index)
        {
            stream.WriteByte(GetKey(index));
            stream.WriteFloat(Rotation.Pitch);
            stream.WriteFloat(Rotation.Yaw);
            stream.WriteFloat(Rotation.Roll);
        }
    }
}
