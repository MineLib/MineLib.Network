using MineLib.Network.Data;
using MineLib.Network.IO;

namespace MineLib.Network.Packets.Server
{
    public struct UseBedPacket : IPacket
    {
        public int EntityID;
        public Vector3 Vector3;

        public const byte PacketID = 0x0A;
        public byte Id { get { return PacketID; } }

        public void ReadPacket(PacketByteReader stream)
        {
            EntityID = stream.ReadInt();
            Vector3.X = stream.ReadInt();
            Vector3.Y = stream.ReadByte();
            Vector3.Z = stream.ReadInt();
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(Id);
            stream.WriteInt(EntityID);
            stream.WriteInt((int)Vector3.X);
            stream.WriteByte((byte)Vector3.Y);
            stream.WriteInt((int)Vector3.Z);
            stream.Purge();
        }
    }
}