using MineLib.Network.IO;
using MineLib.Network.Modern.Data;

namespace MineLib.Network.Modern.Packets.Server
{
    public struct SpawnPaintingPacket : IPacket
    {
        public int EntityID;
        public string Title;
        public Position Location;
        public int Direction;

        public byte ID { get { return 0x10; } }

        public IPacket ReadPacket(MinecraftDataReader reader)
        {
            EntityID = reader.ReadVarInt();
            Title = reader.ReadString();
            Location = Position.FromReaderLong(reader);
            Direction = reader.ReadInt();

            return this;
        }

        public IPacket WritePacket(MinecraftStream stream)
        {
            stream.WriteVarInt(ID);
            stream.WriteVarInt(EntityID);
            stream.WriteString(Title);
            Location.ToStreamLong(stream);
            stream.WriteInt(Direction);
            stream.Purge();

            return this;
        }
    }
}