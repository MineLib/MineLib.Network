using MineLib.Network.Data;
using MineLib.Network.IO;

namespace MineLib.Network.Packets.Server
{
    public struct EntityMetadataPacket : IPacket
    {
        public int EntityID;
        public EntityMetadata Metadata;

        public byte ID { get { return 0x1C; } }

        public void ReadPacket(PacketByteReader reader)
        {
            EntityID = reader.ReadVarInt();
            Metadata = EntityMetadata.FromReader(reader);
        }
    
        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(ID);
            stream.WriteVarInt(EntityID);
            Metadata.ToStream(ref stream);
            stream.Purge();
        }
    }
}