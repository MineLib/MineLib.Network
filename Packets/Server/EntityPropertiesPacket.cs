using MineLib.Network.IO;
using MineLib.Network.Data;

namespace MineLib.Network.Packets.Server
{
    public struct EntityPropertiesPacket : IPacket
    {
        public int EntityID;
        public EntityProperty[] Properties;

        public const byte PacketID = 0x20;
        public byte Id { get { return PacketID; } }

        public void ReadPacket(PacketByteReader stream)
        {
            EntityID = stream.ReadInt();

            var count = stream.ReadInt();

            Properties = new EntityProperty[count];
            for (var i = 0; i < count; i++)
            {
                var property = new EntityProperty();
                property.Key = stream.ReadString();
                property.Value = stream.ReadDouble();
                var listLength = stream.ReadShort();
                property.Modifiers = new Modifiers[listLength];
                for (var j = 0; j < listLength; j++)
                {
                    var item = new Modifiers
                    {
                        UUID_1 = stream.ReadLong(),
                        UUID_2 = stream.ReadLong(),
                        Amount = stream.ReadDouble(),
                        Operation = stream.ReadByte()
                    };

                    property.Modifiers[j] = item;
                }
                Properties[i] = property;
            }
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(Id);
            stream.WriteInt(EntityID);
            stream.WriteInt(Properties.Length);
            for (int i = 0; i < Properties.Length; i++)
            {
                stream.WriteString(Properties[i].Key);
                stream.WriteDouble(Properties[i].Value);
                stream.WriteShort((short)Properties[i].Modifiers.Length);
                for (int j = 0; j < Properties[i].Modifiers.Length; j++)
                {
                    stream.WriteLong(Properties[i].Modifiers[j].UUID_1);
                    stream.WriteLong(Properties[i].Modifiers[j].UUID_2);
                    stream.WriteDouble(Properties[i].Modifiers[j].Amount);
                    stream.WriteByte(Properties[i].Modifiers[j].Operation);
                }
            }
            stream.Purge();
        }
    }
}