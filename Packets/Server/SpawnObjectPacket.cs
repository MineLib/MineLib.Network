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
        public byte Yaw, Pitch;
        public short SpeedX;
        public short SpeedY;
        public short SpeedZ;

        public const byte PacketID = 0x0E;
        public byte Id { get { return PacketID; } }

        public void ReadPacket(PacketByteReader reader)
        {
            EntityID = reader.ReadVarInt();
            Type = (Objects) reader.ReadByte();
            Vector3 = Vector3.FromReaderIntFixedPoint(reader);
            Yaw = reader.ReadByte();
            Pitch = reader.ReadByte();
            SpeedX = reader.ReadShort();
            SpeedY = reader.ReadShort();
            SpeedZ = reader.ReadShort();
        }


        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(Id);
            stream.WriteVarInt(EntityID);
            stream.WriteByte((byte) Type);
            Vector3.ToStreamIntFixedPoint(ref stream);
            stream.WriteByte(Yaw);
            stream.WriteByte(Pitch);
            stream.WriteShort(SpeedX);
            stream.WriteShort(SpeedY);
            stream.WriteShort(SpeedZ);
            stream.Purge();
        }
    }
}