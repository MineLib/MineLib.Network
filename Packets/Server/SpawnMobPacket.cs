using MineLib.Network.Data;
using MineLib.Network.IO;
using MineLib.Network.Enums;

namespace MineLib.Network.Packets.Server
{
    public struct SpawnMobPacket : IPacket
    {
        public int EntityID;
        public Mobs Type;
        public Vector3 Vector3;
        public sbyte Yaw;
        public sbyte Pitch;
        public sbyte HeadPitch;
        public short VelocityX, VelocityY, VelocityZ;
        public EntityMetadata EntityMetadata;

        public byte ID { get { return 0x0F; } }

        public void ReadPacket(PacketByteReader reader)
        {
            EntityID = reader.ReadVarInt();
            Type = (Mobs) reader.ReadByte();
            Vector3 = Vector3.FromReaderIntFixedPoint(reader);
            Yaw = reader.ReadSByte();
            Pitch = reader.ReadSByte();
            HeadPitch = reader.ReadSByte();
            VelocityX = reader.ReadShort();
            VelocityY = reader.ReadShort();
            VelocityZ = reader.ReadShort();
            EntityMetadata = EntityMetadata.FromReader(reader);
        }
    
        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(ID);
            stream.WriteVarInt(EntityID);
            stream.WriteByte((byte) Type);
            Vector3.ToStreamIntFixedPoint(ref stream);
            stream.WriteSByte(Yaw);
            stream.WriteSByte(Pitch);
            stream.WriteSByte(HeadPitch);
            stream.WriteShort(VelocityX);
            stream.WriteShort(VelocityY);
            stream.WriteShort(VelocityZ);
            EntityMetadata.ToStream(ref stream);
            stream.Purge();
        }
    }
}