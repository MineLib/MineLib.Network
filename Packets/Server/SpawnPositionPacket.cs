using MineLib.Network.Data;
using MineLib.Network.IO;

namespace MineLib.Network.Packets.Server
{
    public struct SpawnPositionPacket : IPacket
    {
        public Position Location;

        public byte ID { get { return 0x05; } }

        public void ReadPacket(PacketByteReader reader)
        {
            Location = Position.FromReaderLong(reader);
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(ID);
            Location.ToStreamLong(ref stream);
            stream.Purge();
        }
    }
}