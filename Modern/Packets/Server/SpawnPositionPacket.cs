using MineLib.Network.IO;
using MineLib.Network.Modern.Data;

namespace MineLib.Network.Modern.Packets.Server
{
    public struct SpawnPositionPacket : IPacket
    {
        public Position Location;

        public byte ID { get { return 0x05; } }

        public IPacket ReadPacket(MinecraftDataReader reader)
        {
            Location = Position.FromReaderLong(reader);

            return this;
        }

        public IPacket WritePacket(MinecraftStream stream)
        {
            stream.WriteVarInt(ID);
            Location.ToStreamLong(stream);
            stream.Purge();

            return this;
        }
    }
}