using MineLib.Network.Data;
using MineLib.Network.IO;

namespace MineLib.Network.Packets.Server
{
    public struct SpawnPaintingPacket : IPacket
    {
        public int EntityID;
        public string Title;
        public Position Location;
        public int Direction;

        public const byte PacketID = 0x10;
        public byte Id { get { return PacketID; } }

        public void ReadPacket(PacketByteReader reader)
        {
            EntityID = reader.ReadVarInt();
            Title = reader.ReadString();
            Location = Position.FromReaderLong(reader);
            Direction = reader.ReadInt();
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(Id);
            stream.WriteVarInt(EntityID);
            stream.WriteString(Title);
            Location.ToStreamLong(ref stream);
            stream.WriteInt(Direction);
            stream.Purge();
        }
    }
}