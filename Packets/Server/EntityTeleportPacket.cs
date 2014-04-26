using MineLib.Network.Data;
using MineLib.Network.IO;

namespace MineLib.Network.Packets.Server
{
    public struct EntityTeleportPacket : IPacket
    {
        public int EntityID;
        public Vector3 Vector3;
        public byte Yaw, Pitch;

        public const byte PacketID = 0x18;
        public byte Id { get { return PacketID; } }

        public void ReadPacket(PacketByteReader stream)
        {
            EntityID = stream.ReadInt();
            Vector3.X = stream.ReadInt() / 32;
            Vector3.Y = stream.ReadInt() / 32;
            Vector3.Z = stream.ReadInt() / 32;
            Yaw = stream.ReadByte();
            Pitch = stream.ReadByte();
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(Id);
            stream.WriteInt(EntityID);
            stream.WriteInt((int)(Vector3.X * 32));
            stream.WriteInt((int)(Vector3.Y * 32));
            stream.WriteInt((int)(Vector3.Z * 32));
            stream.WriteByte(Yaw);
            stream.WriteByte(Pitch);
            stream.Purge();
        }
    }
}