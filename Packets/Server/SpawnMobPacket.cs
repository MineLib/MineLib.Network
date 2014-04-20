using MineLib.Network.IO;
using MineLib.Network.Data.EntityMetadata;
using MineLib.Network.Enums;


namespace MineLib.Network.Packets.Server
{
    public struct SpawnMobPacket : IPacket
    {
        public int EntityID;
        public Mobs Type;
        public double X, Y, Z;
        public byte Pitch, HeadPitch, Yaw;
        public short VelocityX, VelocityY, VelocityZ;
        public MetadataDictionary Metadata;

        public const byte PacketID = 0x0F;
        public byte Id { get { return PacketID; } }
    
        public void ReadPacket(PacketByteReader stream)
        {
            EntityID = stream.ReadVarInt();
            Type = (Mobs)stream.ReadByte();
            X = stream.ReadInt() / 32;
            Y = stream.ReadInt() / 32;
            Z = stream.ReadInt() / 32;
            Pitch = stream.ReadByte();
            HeadPitch = stream.ReadByte();
            Yaw = stream.ReadByte();
            VelocityX = stream.ReadShort();
            VelocityY = stream.ReadShort();
            VelocityZ = stream.ReadShort();
            Metadata = MetadataDictionary.FromStream(ref stream);
        }
    
        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(Id);
            stream.WriteVarInt(EntityID);
            stream.WriteByte((byte)Type);
            stream.WriteInt((int)X * 32);
            stream.WriteInt((int)Y * 32);
            stream.WriteInt((int)Z * 32);
            stream.WriteByte(Pitch);
            stream.WriteByte(HeadPitch);
            stream.WriteByte(Yaw);
            stream.WriteShort(VelocityX);
            stream.WriteShort(VelocityY);
            stream.WriteShort(VelocityZ);
            Metadata.WriteTo(ref stream);
            stream.Purge();
        }
    }
}