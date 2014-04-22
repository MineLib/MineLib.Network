using MineLib.Network.Data;
using MineLib.Network.IO;

namespace MineLib.Network.Packets.Server
{
    public struct BlockChangePacket : IPacket
    {
        public Vector3 Vector3;
        public int BlockID;
        public byte BlockMetadata;

        public const byte PacketID = 0x23;
        public byte Id { get { return PacketID; } }

        public void ReadPacket(PacketByteReader stream)
        {
            Vector3.X = stream.ReadInt();
            Vector3.Y = stream.ReadByte();
            Vector3.Z = stream.ReadInt();
            BlockID = stream.ReadVarInt();
            BlockMetadata = stream.ReadByte();
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(Id);
            stream.WriteInt((int)Vector3.X);
            stream.WriteByte((byte)Vector3.Y);
            stream.WriteInt((int)Vector3.Z);
            stream.WriteVarInt(BlockID);
            stream.WriteByte(BlockMetadata);
            stream.Purge();
        }
    }
}