using MineLib.Network.Data;
using MineLib.Network.IO;

namespace MineLib.Network.Packets.Server
{
    public struct BlockChangePacket : IPacket
    {
        public Coordinates3D Coordinates;
        public int BlockID;
        public byte BlockMetadata;

        public const byte PacketID = 0x23;
        public byte Id { get { return PacketID; } }

        public void ReadPacket(PacketByteReader stream)
        {
            Coordinates.X = stream.ReadInt();
            Coordinates.Y = stream.ReadByte();
            Coordinates.Z = stream.ReadInt();
            BlockID = stream.ReadVarInt();
            BlockMetadata = stream.ReadByte();
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(Id);
            stream.WriteInt(Coordinates.X);
            stream.WriteByte((byte)Coordinates.Y);
            stream.WriteInt(Coordinates.Z);
            stream.WriteVarInt(BlockID);
            stream.WriteByte(BlockMetadata);
            stream.Purge();
        }
    }
}