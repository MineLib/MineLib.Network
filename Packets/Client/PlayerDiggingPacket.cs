using CWrapped;
using MineLib.Network.Enums;

namespace MineLib.Network.Packets.Client
{
    public struct PlayerDiggingPacket : IPacket
    {
        public DigStatus Action;
        public int X;
        public byte Y;
        public int Z;
        public byte Face;

        public const byte PacketId = 0x0E;
        public byte Id { get { return 0x0E; } }

        public void ReadPacket(ref Wrapped stream)
        {
            Action = (DigStatus)stream.ReadVarInt();
            X = stream.ReadShort();
            Y = stream.ReadByte();
            Z = stream.ReadShort();
            Face = stream.ReadByte();
        }

        public void WritePacket(ref Wrapped stream)
        {
            stream.WriteVarInt(Id);
            stream.WriteVarInt((byte)Action);
            stream.WriteVarInt(X);
            stream.WriteVarInt(Y);
            stream.WriteVarInt(Z);
            stream.WriteVarInt(Face);
            stream.Purge();
        }
    }
}