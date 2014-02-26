using CWrapped;

namespace MineLib.Network.Packets.Server
{
    public struct EntityHeadLookPacket : IPacket
    {
        public int EntityID;
        public byte HeadYaw;

        public const byte PacketId = 0x19;
        public byte Id { get { return 0x19; } }

        public void ReadPacket(ref Wrapped stream)
        {
            EntityID = stream.ReadInt();
            HeadYaw = stream.ReadByte();
        }

        public void WritePacket(ref Wrapped stream)
        {
            stream.WriteVarInt(Id);
            stream.WriteInt(EntityID);
            stream.WriteByte(HeadYaw);
            stream.Purge();
        }
    }
}