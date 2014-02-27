using CWrapped;
using MineLib.Network.Enums;

namespace MineLib.Network.Packets.Client
{
    public struct EntityActionPacket : IPacket
    {
        public int EntityID;
        public EntityAction Action;
        public int JumpBoost;

        public const byte PacketId = 0x0B;
        public byte Id { get { return 0x0B; } }

        public void ReadPacket(ref Wrapped stream)
        {
            EntityID = stream.ReadInt();
            Action = (EntityAction)stream.ReadByte();
            JumpBoost = stream.ReadInt();
        }

        public void WritePacket(ref Wrapped stream)
        {
            stream.WriteVarInt(Id);
            stream.WriteInt(EntityID);
            stream.WriteByte((byte)Action);
            stream.WriteInt(JumpBoost);
            stream.Purge();
        }
    }
}