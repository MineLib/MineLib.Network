using MineLib.Network.Data;
using MineLib.Network.IO;

namespace MineLib.Network.Packets.Server
{
    public struct SpawnGlobalEntityPacket : IPacket
    {
        public int EntityID;
        public byte Type;
        public Vector3 Vector3;

        public const byte PacketID = 0x2C;
        public byte Id { get { return PacketID; } }

        public void ReadPacket(PacketByteReader stream)
        {
            EntityID = stream.ReadVarInt();
            Type = stream.ReadByte();
            Vector3.X = stream.ReadInt() / 32;
            Vector3.Y = stream.ReadInt() / 32;
            Vector3.Z = stream.ReadInt() / 32;
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(Id);
            stream.WriteVarInt(EntityID);
            stream.WriteByte(Type);
            stream.WriteInt((int)(Vector3.X * 32));
            stream.WriteInt((int)(Vector3.Y * 32));
            stream.WriteInt((int)(Vector3.Z * 32));
            stream.Purge();
        }
    }
}