using MineLib.Network.IO;
using MineLib.Network.Data.EntityMetadata;

namespace MineLib.Network.Packets.Server
{
    public struct EntityMetadataPacket : IPacket
    {
        public int EntityID;
        public MetadataDictionary Metadata;

        public const byte PacketID = 0x1C;
        public byte Id { get { return PacketID; } }
    
        public void ReadPacket(PacketByteReader stream)
        {
            EntityID = stream.ReadInt();
            Metadata = MetadataDictionary.FromStream(stream);
        }
    
        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(Id);
            stream.WriteInt(EntityID);
            Metadata.WriteTo(ref stream);
            stream.Purge();
        }
    }
}