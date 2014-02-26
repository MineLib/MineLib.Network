using CWrapped;
using MineLib.Network.Enums;

namespace MineLib.Network.Packets.Server
{
    public struct EntityStatusPacket : IPacket
    {
        public int EntityID;
        public EntityStatus Status;

        public const byte PacketId = 0x1A;
        public byte Id { get { return 0x1A; } }

        public void ReadPacket(ref Wrapped stream)
        {
            EntityID = stream.ReadInt();
            Status = (EntityStatus)stream.ReadByte();
        }

        public void WritePacket(ref Wrapped stream)
        {
            stream.WriteVarInt(Id);
            stream.WriteInt(EntityID);
            stream.WriteByte((byte)Status);
            stream.Purge();
        }
    }
}