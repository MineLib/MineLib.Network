using MineLib.Network.IO;
using MineLib.Network.Modern.Data;

namespace MineLib.Network.Modern.Packets.Server
{
    public struct EntityPropertiesPacket : IPacket
    {
        public int EntityID;
        public EntityPropertyList EntityProperties;

        public byte ID { get { return 0x20; } }

        public IPacket ReadPacket(IMinecraftDataReader reader)
        {
            EntityID = reader.ReadVarInt();
            EntityProperties = EntityPropertyList.FromReader(reader);

            return this;
        }

        public IPacket WritePacket(IMinecraftStream stream)
        {
            stream.WriteVarInt(ID);
            stream.WriteVarInt(EntityID);
            EntityProperties.ToStream(ref stream);
            stream.Purge();

            return this;
        }
    }
}