using MineLib.Network.Data;
using MineLib.Network.IO;

namespace MineLib.Network.Packets.Server
{
    public struct MapsPacket : IPacket
    {
        public int ItemDamage;
        public sbyte Scale;
        public Icons Icons;
        public sbyte Columns;
        public sbyte Rows;
        public sbyte X, Y;
        public byte[] Data; // TODO: Parse dat shiet

        public const byte PacketID = 0x34;
        public byte Id { get { return PacketID; } }

        public void ReadPacket(PacketByteReader reader)
        {
            ItemDamage = reader.ReadVarInt();
            Scale = reader.ReadSByte();
            Icons = Icons.FromReader(reader);
            Columns = reader.ReadSByte();

            if (Columns > 0)
            {
                Rows = reader.ReadSByte();
                X = reader.ReadSByte();
                Y = reader.ReadSByte();
                var dataLength = reader.ReadShort();
                Data = reader.ReadByteArray(dataLength);
            }
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(Id);
            stream.WriteVarInt(ItemDamage);
            stream.WriteSByte(Scale);
            Icons.ToStream(ref stream);
            stream.WriteSByte(Columns);
            if (Columns > 0)
            {
                stream.WriteSByte(Rows);
                stream.WriteSByte(X);
                stream.WriteSByte(Y);
                stream.WriteVarInt(Data.Length);
                stream.WriteByteArray(Data);
            }
            stream.Purge();
        }
    }
}