using MineLib.Network.Data;
using MineLib.Network.IO;

namespace MineLib.Network.Packets.Server
{
    public struct UseBedPacket : IPacket
    {
        public int EntityID;
        public Position Location;

        public byte ID { get { return 0x0A; } }

        public void ReadPacket(PacketByteReader reader)
        {
            EntityID = reader.ReadVarInt();
            Location = Position.FromReaderLong(reader);
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(ID);
            stream.WriteVarInt(EntityID);
            Location.ToStreamLong(ref stream);
            stream.Purge();
        }
    }
}