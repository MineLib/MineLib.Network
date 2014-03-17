using System;
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

        public void ReadPacket(ref Wrapped stream)
        {
            EntityID = stream.ReadInt();
            var count = stream.ReadInt();
            if (count < 0)
                throw new InvalidOperationException("Cannot specify less than zero properties.");
            Properties = new EntityProperty[count];
            for (int i = 0; i < count; i++)
            {
                var property = new EntityProperty { Key = stream.ReadString(), Value = stream.ReadDouble() };

                var listLength = stream.ReadShort();
                property.UnknownList = new EntityPropertyListItem[listLength];
                for (int j = 0; j < listLength; j++)
                {
                    var item = new EntityPropertyListItem
                    {
                        UnknownMSB = stream.ReadLong(),
                        UnknownLSB = stream.ReadLong(),
                        UnknownDouble = stream.ReadDouble(),
                        UnknownByte = stream.ReadByte()
                    };

                    property.UnknownList[j] = item;
                }
                Properties[i] = property;
            }
        }

        public void WritePacket(ref Wrapped stream)
        {
            stream.WriteVarInt(Id);
            stream.WriteInt(EntityID);
            stream.WriteInt(Properties.Length);
            for (int i = 0; i < Properties.Length; i++)
            {
                stream.WriteString(Properties[i].Key);
                stream.WriteDouble(Properties[i].Value);
                stream.WriteShort((short)Properties[i].UnknownList.Length);
                for (int j = 0; j < Properties[i].UnknownList.Length; j++)
                {
                    stream.WriteLong(Properties[i].UnknownList[j].UnknownMSB);
                    stream.WriteLong(Properties[i].UnknownList[j].UnknownLSB);
                    stream.WriteDouble(Properties[i].UnknownList[j].UnknownDouble);
                    stream.WriteByte(Properties[i].UnknownList[j].UnknownByte);
                }
            }
            stream.Purge();
        }
    }
}