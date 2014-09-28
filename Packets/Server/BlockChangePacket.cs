using MineLib.Network.Data;
using MineLib.Network.IO;

namespace MineLib.Network.Packets.Server
{
    public struct BlockChangePacket : IPacket
    {
        public Position Location;
        public int BlockIDMeta;

        public byte ID { get { return 0x23; } }

        public void ReadPacket(PacketByteReader reader)
        {
            Location = Position.FromLong(reader.ReadLong());
            BlockIDMeta = reader.ReadVarInt();
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(ID);
            Location.ToStreamLong(ref stream);
            stream.WriteVarInt(BlockIDMeta);
            stream.Purge();
        }
    }
}