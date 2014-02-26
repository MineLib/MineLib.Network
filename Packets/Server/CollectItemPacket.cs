using CWrapped;

namespace MineLib.Network.Packets.Server
{
    public struct CollectItemPacket : IPacket
    {
        public int CollectedEntityID;
        public int CollectorEntityID;

        public const byte PacketId = 0x0D;
        public byte Id { get { return 0x0D; } }

        public void ReadPacket(ref Wrapped stream)
        {
            CollectedEntityID = stream.ReadInt();
            CollectorEntityID = stream.ReadInt();
        }

        public void WritePacket(ref Wrapped stream)
        {
            stream.WriteVarInt(Id);
            stream.WriteInt(CollectedEntityID);
            stream.WriteInt(CollectorEntityID);
            stream.Purge();
        }
    }
}