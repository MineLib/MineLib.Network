using MineLib.Network.Data.Structs;
using MineLib.Network.IO;

namespace MineLib.Network.Modern.Packets.Server
{
    public struct EntityMetadataPacket : IPacket
    {
        public int EntityID;
        public EntityMetadataList Metadata;

        public byte ID { get { return 0x1C; } }

        public IPacket ReadPacket(IMinecraftDataReader reader)
        {
            EntityID = reader.ReadVarInt();
            Metadata = EntityMetadataList.FromReader(reader);

            return this;
        }
    
        public IPacket WritePacket(IMinecraftStream stream)
        {
            stream.WriteVarInt(ID);
            stream.WriteVarInt(EntityID);
            Metadata.ToStream(stream);
            stream.Purge();

            return this;
        }
    }
}