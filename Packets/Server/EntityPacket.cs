using CWrapped;

namespace MineLib.Network.Packets.Server
{
    public struct EntityPacket : IPacket
    {
        public int EntityID;

        public const byte PacketId = 0x14;
        public byte Id { get { return 0x14; } }

        public void ReadPacket(ref Wrapped stream)
        {
            EntityID = stream.ReadInt();
        }

        public void WritePacket(ref Wrapped stream)
        {
            stream.WriteVarInt(Id);
            stream.WriteInt(EntityID);
            stream.Purge();
        }
    }
}