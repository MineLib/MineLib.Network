using MineLib.Network.IO;

namespace MineLib.Network.Packets.Server
{
    public struct CollectItemPacket : IPacket
    {
        public int CollectedEntityID;
        public int CollectorEntityID;

        public const byte PacketID = 0x0D;
        public byte Id { get { return PacketID; } }

        public void ReadPacket(PacketByteReader reader)
        {
            CollectedEntityID = reader.ReadVarInt();
            CollectorEntityID = reader.ReadVarInt();
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(Id);
            stream.WriteVarInt(CollectedEntityID);
            stream.WriteVarInt(CollectorEntityID);
            stream.Purge();
        }
    }
}