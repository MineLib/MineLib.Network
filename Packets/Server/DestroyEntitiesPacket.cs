using MineLib.Network.IO;


namespace MineLib.Network.Packets.Server
{
    public struct DestroyEntitiesPacket : IPacket
    {
        public int[] EntityIDs;

        public const byte PacketID = 0x13;
        public byte Id { get { return PacketID; } }

        public void ReadPacket(PacketByteReader stream)
        {
            var length = stream.ReadVarInt();
            EntityIDs = stream.ReadIntArray(length);
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(Id);
            stream.WriteVarInt(EntityIDs.Length);
            stream.WriteIntArray(EntityIDs);
            stream.Purge();
        }
    }
}