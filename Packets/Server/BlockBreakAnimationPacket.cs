using MineLib.Network.Data;
using MineLib.Network.IO;

namespace MineLib.Network.Packets.Server
{
    public struct BlockBreakAnimationPacket : IPacket
    {
        public int EntityID;
        public Vector3 Vector3;
        public byte DestroyStage;

        public const byte PacketID = 0x25;
        public byte Id { get { return PacketID; } }

        public void ReadPacket(PacketByteReader stream)
        {
            EntityID = stream.ReadVarInt();
            Vector3.X = stream.ReadInt();
            Vector3.Y = stream.ReadInt();
            Vector3.Z = stream.ReadInt();
            DestroyStage = stream.ReadByte();
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(Id);
            stream.WriteVarInt(EntityID);
            stream.WriteInt((int)Vector3.X);
            stream.WriteInt((int)Vector3.Y);
            stream.WriteInt((int)Vector3.Z);
            stream.WriteByte(DestroyStage);
            stream.Purge();
        }
    }
}