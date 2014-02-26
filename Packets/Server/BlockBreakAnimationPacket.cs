using CWrapped;

namespace MineLib.Network.Packets.Server
{
    public struct BlockBreakAnimationPacket : IPacket
    {
        public int EntityID;
        public int X, Y, Z;
        public byte DestroyStage;

        public const byte PacketId = 0x25;
        public byte Id { get { return 0x25; } }

        public void ReadPacket(ref Wrapped stream)
        {
            EntityID = stream.ReadVarInt();
            X = stream.ReadInt();
            Y = stream.ReadInt();
            Z = stream.ReadInt();
            DestroyStage = stream.ReadByte();
        }

        public void WritePacket(ref Wrapped stream)
        {
            stream.WriteVarInt(Id);
            stream.WriteVarInt(EntityID);
            stream.WriteInt(X);
            stream.WriteInt(Y);
            stream.WriteInt(Z);
            stream.WriteByte(DestroyStage);
            stream.Purge();
        }
    }
}