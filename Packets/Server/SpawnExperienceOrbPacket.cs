using MineLib.Network.Data;
using MineLib.Network.IO;

namespace MineLib.Network.Packets.Server
{
    public struct SpawnExperienceOrbPacket : IPacket
    {
        public int EntityID;
        public Vector3 Vector3;
        public short Count;

        public const byte PacketID = 0x11;
        public byte Id { get { return PacketID; } }

        public void ReadPacket(PacketByteReader stream)
        {
            EntityID = stream.ReadVarInt();
            Vector3.X = stream.ReadInt();
            Vector3.Y = stream.ReadInt();
            Vector3.Z = stream.ReadInt();
            Count = stream.ReadShort();
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(Id);
            stream.WriteVarInt(EntityID);
            stream.WriteInt((int)Vector3.X);
            stream.WriteInt((int)Vector3.Y);
            stream.WriteInt((int)Vector3.Z);
            stream.WriteShort(Count);
            stream.Purge();
        }
    }
}