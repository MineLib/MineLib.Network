using MineLib.Network.IO;
using MineLib.Network.Main.Data;

namespace MineLib.Network.Main.Packets.Server
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