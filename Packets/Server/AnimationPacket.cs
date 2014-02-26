using CWrapped;
using MineLib.Network.Enums;

namespace MineLib.Network.Packets.Server
{
    public struct AnimationPacket : IPacket
    {
        public int EntityID;
        public Animation Animation;

        public const byte PacketId = 0x0B;
        public byte Id { get { return 0x0B; } }

        public void ReadPacket(ref Wrapped stream)
        {
            EntityID = stream.ReadVarInt();
            Animation = (Animation)stream.ReadByte();
        }

        public void WritePacket(ref Wrapped stream)
        {
            stream.WriteVarInt(Id);
            stream.WriteVarInt(EntityID);
            stream.WriteByte((byte)Animation);
            stream.Purge();
        }
    }
}