using MineLib.Network.Data;
using MineLib.Network.IO;
using MineLib.Network.Enums;

namespace MineLib.Network.Packets.Client
{
    public struct PlayerDiggingPacket : IPacket
    {
        public BlockStatus Status;
        public Vector3 Vector3;
        public byte Face;

        public const byte PacketID = 0x07;
        public byte Id { get { return PacketID; } }

        public void ReadPacket(PacketByteReader stream)
        {
            Status = (BlockStatus)stream.ReadByte();
            Vector3.X = stream.ReadInt();
            Vector3.Y = stream.ReadByte();
            Vector3.Z = stream.ReadInt();
            Face = stream.ReadByte();
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(Id);
            stream.WriteByte((byte)Status);
            stream.WriteInt((int)Vector3.X);
            stream.WriteByte((byte)Vector3.Y);
            stream.WriteInt((int)Vector3.Z);
            stream.WriteByte(Face);
            stream.Purge();
        }
    }
}