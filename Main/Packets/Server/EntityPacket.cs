using MineLib.Network.IO;

namespace MineLib.Network.Main.Packets.Server
{
    public struct EntityPacket : IPacket
    {
        public int EntityID;

        public byte ID { get { return 0x14; } }

        public void ReadPacket(PacketByteReader reader)
        {
            EntityID = reader.ReadVarInt();
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(ID);
            stream.WriteVarInt(EntityID);
            stream.Purge();
        }
    }
}