using MineLib.Network.Data;
using MineLib.Network.IO;
using MineLib.Network.Enums;

namespace MineLib.Network.Packets.Server
{
    public struct SpawnObjectPacket : IPacket
    {
        public int EntityID;
        public Objects Type;
        public Vector3 Vector3;
        public int Data; // Maybe new data-type ObjectData?
        public short? SpeedX, SpeedY, SpeedZ;
        public byte Yaw, Pitch;

        public const byte PacketID = 0x0E;
        public byte Id { get { return PacketID; } }

        public void ReadPacket(PacketByteReader stream)
        {
            EntityID = stream.ReadVarInt();
            Type = (Objects)stream.ReadByte();
            Vector3.X = stream.ReadInt() / 32;
            Vector3.Y = stream.ReadInt() / 32;
            Vector3.Z = stream.ReadInt() / 32;
            Yaw = stream.ReadByte();
            Pitch = stream.ReadByte();
            Data = stream.ReadInt();
            if (Data != 0)
            {
                SpeedX = stream.ReadShort();
                SpeedY = stream.ReadShort();
                SpeedZ = stream.ReadShort();
            }
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(Id);
            stream.WriteVarInt(EntityID);
            stream.WriteByte((byte)Type);
            stream.WriteInt((int)Vector3.X * 32);
            stream.WriteInt((int)Vector3.Y * 32);
            stream.WriteInt((int)Vector3.Z * 32);
            stream.WriteByte(Yaw);
            stream.WriteByte(Pitch);
            stream.WriteInt(Data);
            if (Data != 0)
            {
                stream.WriteShort(SpeedX.Value);
                stream.WriteShort(SpeedY.Value);
                stream.WriteShort(SpeedZ.Value);
            }
            stream.Purge();
        }
    }
}