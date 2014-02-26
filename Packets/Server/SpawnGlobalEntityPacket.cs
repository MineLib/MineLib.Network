using CWrapped;

namespace MineLib.Network.Packets.Server
{
    public struct SpawnGlobalEntityPacket : IPacket
    {
        public int EntityID;
        public byte Type;
        public int X, Y, Z;

        public const byte PacketId = 0x2C;
        public byte Id { get { return 0x2C; } }

        public void ReadPacket(ref Wrapped stream)
        {
            EntityID = stream.ReadVarInt();
            Type = stream.ReadByte();
            X = stream.ReadInt();
            Y = stream.ReadInt();
            Z = stream.ReadInt();
        }

        public void WritePacket(ref Wrapped stream)
        {
            stream.WriteVarInt(Id);
            stream.WriteVarInt(EntityID);
            stream.WriteByte(Type);
            stream.WriteInt(X);
            stream.WriteInt(Y);
            stream.WriteInt(Z);
            stream.Purge();
        }
    }
}