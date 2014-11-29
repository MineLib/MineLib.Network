using MineLib.Network.Data;
using MineLib.Network.Data.Structs;
using MineLib.Network.IO;
using MineLib.Network.Modern.Enums;

namespace MineLib.Network.Modern.Packets.Server
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
        public EntityMetadataList EntityMetadata;

        public byte ID { get { return 0x0F; } }

        public IPacket ReadPacket(IMinecraftDataReader reader)
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
            EntityMetadata = EntityMetadataList.FromReader(reader);

            return this;
        }
    
        public IPacket WritePacket(IMinecraftStream stream)
        {
            stream.WriteVarInt(ID);
            stream.WriteVarInt(EntityID);
            stream.WriteByte((byte) Type);
            Vector3.ToStreamIntFixedPoint(stream);
            stream.WriteSByte(Yaw);
            stream.WriteSByte(Pitch);
            stream.WriteSByte(HeadPitch);
            stream.WriteShort(VelocityX);
            stream.WriteShort(VelocityY);
            stream.WriteShort(VelocityZ);
            EntityMetadata.ToStream(stream);
            stream.Purge();

            return this;
        }
    }
}