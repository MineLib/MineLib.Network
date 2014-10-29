using MineLib.Network.IO;

namespace MineLib.Network.Modern.Data.EntityMetadataEntries
{
    /// <summary>
    /// Vector(Position) Metadata
    /// </summary>
    public class MetadataVector : MetadataEntry
    {
        public override byte Identifier { get { return 6; } }
        public override string FriendlyName { get { return "vector"; } }

        public Position Coordinates;

        public MetadataVector()
        {
        }

        public MetadataVector(int x, int y, int z)
        {
            Coordinates.X = x;
            Coordinates.Y = y;
            Coordinates.Z = z;
        }

        public MetadataVector(Position position)
        {
            Coordinates = position;
        }

        public override void FromReader(IMinecraftDataReader reader)
        {
            Coordinates.X = reader.ReadInt();
            Coordinates.Y = reader.ReadInt();
            Coordinates.Z = reader.ReadInt();
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
