using MineLib.Network.IO;
using MineLib.Network.Main.Data;

namespace MineLib.Network.Main.Packets.Server
{
    public struct SpawnPaintingPacket : IPacket
    {
        public int EntityID;
        public string Title;
        public Position Location;
        public int Direction;

        public byte ID { get { return 0x10; } }

        public void ReadPacket(PacketByteReader reader)
        {
            EntityID = reader.ReadVarInt();
            Title = reader.ReadString();
            Location = Position.FromReaderLong(reader);
            Direction = reader.ReadInt();
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(ID);
            stream.WriteVarInt(EntityID);
            stream.WriteString(Title);
            Location.ToStreamLong(ref stream);
            stream.WriteInt(Direction);
            stream.Purge();
        }
    }
}