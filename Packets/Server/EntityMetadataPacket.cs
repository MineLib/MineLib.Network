using MineLib.Network.Data;
using MineLib.Network.IO;

namespace MineLib.Network.Packets.Server
{
    public struct EntityMetadataPacket : IPacket
    {
        public int EntityID;
        public EntityMetadata Metadata;

        public const byte PacketID = 0x1C;
        public byte Id { get { return PacketID; } }

        public void ReadPacket(PacketByteReader reader)
        {
            EntityID = reader.ReadVarInt();
            Metadata = EntityMetadata.FromReader(reader);
        }
    
        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(Id);
            stream.WriteVarInt(EntityID);
            Metadata.ToStream(ref stream);
            stream.Purge();
        }
    }
}