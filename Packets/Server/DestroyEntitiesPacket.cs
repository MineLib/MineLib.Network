using CWrapped;

namespace MineLib.Network.Packets.Server
{
    public struct DestroyEntitiesPacket : IPacket
    {
        public int[] EntityIDs;

        public const byte PacketID = 0x13;
        public byte Id { get { return PacketID; } }

        public void ReadPacket(ref Wrapped stream)
        {
            var length = stream.ReadVarInt();
            EntityIDs = stream.ReadIntArray(length);
        }

        public void WritePacket(ref Wrapped stream)
        {
            stream.WriteVarInt(Id);
            stream.WriteVarInt(EntityIDs.Length);
            stream.WriteIntArray(EntityIDs);
            stream.Purge();
        }
    }
}