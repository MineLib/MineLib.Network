using MineLib.Network.IO;


namespace MineLib.Network.Packets.Server
{
    public struct CollectItemPacket : IPacket
    {
        public int CollectedEntityID;
        public int CollectorEntityID;

        public const byte PacketID = 0x0D;
        public byte Id { get { return PacketID; } }

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