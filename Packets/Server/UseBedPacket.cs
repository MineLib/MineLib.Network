using CWrapped;

namespace MineLib.Network.Packets.Server
{
    public struct UseBedPacket : IPacket
    {
        public int EntityID;
        public int X;
        public byte Y;
        public int Z;

        public const byte PacketId = 0x0A;
        public byte Id { get { return 0x0A; } }

        public void ReadPacket(ref Wrapped stream)
        {
            EntityID = stream.ReadInt();
            X = stream.ReadInt();
            Y = stream.ReadByte();
            Z = stream.ReadInt();
        }

        public void WritePacket(ref Wrapped stream)
        {
            stream.WriteVarInt(Id);
            stream.WriteInt(EntityID);
            stream.WriteInt(X);
            stream.WriteByte(Y);
            stream.WriteInt(Z);
            stream.Purge();
        }
    }
}