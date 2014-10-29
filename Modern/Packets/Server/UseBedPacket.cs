using MineLib.Network.IO;
using MineLib.Network.Modern.Data;

namespace MineLib.Network.Modern.Packets.Server
{
    public struct UseBedPacket : IPacket
    {
        public int EntityID;
        public Position Location;

        public byte ID { get { return 0x0A; } }

        public IPacket ReadPacket(IMinecraftDataReader reader)
        {
            EntityID = reader.ReadVarInt();
            Location = Position.FromReaderLong(reader);

            return this;
        }

        public IPacket WritePacket(IMinecraftStream stream)
        {
            stream.WriteVarInt(ID);
            stream.WriteVarInt(EntityID);
            Location.ToStreamLong(stream);
            stream.Purge();

            return this;
        }
    }
}