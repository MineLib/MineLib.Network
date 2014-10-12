using MineLib.Network.IO;

namespace MineLib.Network.Modern.Packets.Server
{
    public struct CollectItemPacket : IPacket
    {
        public int CollectedEntityID;
        public int CollectorEntityID;

        public byte ID { get { return 0x0D; } }

        public void ReadPacket(PacketByteReader reader)
        {
            CollectedEntityID = reader.ReadVarInt();
            CollectorEntityID = reader.ReadVarInt();
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(ID);
            stream.WriteVarInt(CollectedEntityID);
            stream.WriteVarInt(CollectorEntityID);
            stream.Purge();
        }
    }
}