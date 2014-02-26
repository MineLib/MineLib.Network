using CWrapped;

namespace MineLib.Network.Packets.Server
{
    public struct SpawnPositionPacket : IPacket
    {
        public int X, Y, Z;

        public const byte PacketId = 0x05;
        public byte Id { get { return 0x05; } }

        public void ReadPacket(ref Wrapped stream)
        {
            X = stream.ReadInt();
            Y = stream.ReadInt();
            Z = stream.ReadInt();
        }

        public void WritePacket(ref Wrapped stream)
        {
            stream.WriteVarInt(Id);
            stream.WriteInt(X);
            stream.WriteInt(Y);
            stream.WriteInt(Z);
            stream.Purge();
        }
    }
}