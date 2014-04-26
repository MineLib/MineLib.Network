using MineLib.Network.Data;
using MineLib.Network.IO;
using MineLib.Network.Enums;

namespace MineLib.Network.Packets.Client
{
    public struct PlayerDiggingPacket : IPacket
    {
        public BlockStatus Status;
        public Coordinates3D Coordinates;
        public byte Face;

        public const byte PacketID = 0x07;
        public byte Id { get { return PacketID; } }

        public void ReadPacket(PacketByteReader stream)
        {
            Status = (BlockStatus)stream.ReadByte();
            Coordinates.X = stream.ReadInt();
            Coordinates.Y = stream.ReadByte();
            Coordinates.Z = stream.ReadInt();
            Face = stream.ReadByte();
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(Id);
            stream.WriteByte((byte)Status);
            stream.WriteInt(Coordinates.X);
            stream.WriteByte((byte)Coordinates.Y);
            stream.WriteInt(Coordinates.Z);
            stream.WriteByte(Face);
            stream.Purge();
        }
    }
}