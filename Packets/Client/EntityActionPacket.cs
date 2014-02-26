using CWrapped;
using MineLib.Network.Enums;

namespace MineLib.Network.Packets.Client
{
    public struct EntityActionPacket : IPacket
    {
        public int EntityID;
        public EntityAction Action;
        public int Unknown;

        public const byte PacketId = 0x13;
        public byte Id { get { return 0x13; } }

        public void ReadPacket(ref Wrapped stream)
        {
            EntityID = stream.ReadShort();
            Action = (EntityAction)stream.ReadVarInt();
            Unknown = stream.ReadShort();
        }

        public void WritePacket(ref Wrapped stream)
        {
            stream.WriteVarInt(Id);
            stream.WriteVarInt(EntityID);
            stream.WriteVarInt((byte)Action);
            stream.WriteVarInt(Unknown);
            stream.Purge();
        }
    }
}