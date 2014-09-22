using MineLib.Network.IO;
using MineLib.Network.Data;

namespace MineLib.Network.Packets.Server
{
    public struct EntityPropertiesPacket : IPacket
    {
        public int EntityID;
        public EntityProperty Properties;

        public const byte PacketID = 0x20;
        public byte Id { get { return PacketID; } }

        public void ReadPacket(PacketByteReader reader)
        {
            EntityID = reader.ReadVarInt();
            Properties = EntityProperty.FromReader(reader);
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(Id);
            stream.WriteVarInt(EntityID);
            Properties.ToStream(ref stream);
            stream.Purge();
        }
    }
}