using MineLib.Network.IO;

namespace MineLib.Network.Packets.Server
{
    public struct CollectItemPacket : IPacket
    {
        public int CollectedEntityID;
        public int CollectorEntityID;

        public const byte PacketID = 0x0D;
        public byte Id { get { return PacketID; } }

        public void ReadPacket(PacketByteReader stream)
        {
            CollectedEntityID = stream.ReadInt();
            CollectorEntityID = stream.ReadInt();
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(Id);
            stream.WriteInt(CollectedEntityID);
            stream.WriteInt(CollectorEntityID);
            stream.Purge();
        }
    }
}