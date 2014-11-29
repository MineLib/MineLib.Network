using MineLib.Network.IO;

namespace MineLib.Network.Data.EntityMetadata
{
    /// <summary>
    /// Vector(Position) Metadata
    /// </summary>
    public class EntityMetadataVector : EntityMetadataEntry
    {
        public override byte Identifier { get { return 6; } }
        public override string FriendlyName { get { return "vector"; } }

        public Position Coordinates;

        public EntityMetadataVector()
        {
        }

        public EntityMetadataVector(int x, int y, int z)
        {
            Coordinates = new Position(x, y, z);
        }

        public EntityMetadataVector(Position position)
        {
            Coordinates = position;
        }

        public override void FromReader(IMinecraftDataReader reader)
        {
            Coordinates = new Position(
                reader.ReadInt(),
                reader.ReadInt(),
                reader.ReadInt());
        }

        public override void ToStream(IMinecraftStream stream, byte index)
        {
            stream.WriteByte(GetKey(index));
            stream.WriteInt(Coordinates.X);
            stream.WriteInt(Coordinates.Y);
            stream.WriteInt(Coordinates.Z);
        }
    }
}
