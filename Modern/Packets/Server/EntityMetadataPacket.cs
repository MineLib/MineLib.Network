using MineLib.Network.IO;
using MineLib.Network.Modern.Data;

namespace MineLib.Network.Modern.Packets.Server
{
    public struct EntityMetadataPacket : IPacket
    {
        public int EntityID;
        public EntityMetadata Metadata;

        public byte ID { get { return 0x1C; } }

        public IPacket ReadPacket(MinecraftDataReader reader)
        {
            EntityID = reader.ReadVarInt();
            Metadata = EntityMetadata.FromReader(reader);

            return this;
        }
    
        public IPacket WritePacket(MinecraftStream stream)
        {
            stream.WriteVarInt(ID);
            stream.WriteVarInt(EntityID);
            Metadata.ToStream(stream);
            stream.Purge();

            return this;
        }
    }
}