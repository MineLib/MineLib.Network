using MineLib.Network.IO;
using MineLib.Network.Data.EntityMetadata;
using MineLib.Network.Enums;


namespace MineLib.Network.Packets.Server
{
    public struct SpawnMobPacket : IPacket
    {
        public int EntityID;
        public Mobs Type;
        public int X, Y, Z;
        public byte Pitch, HeadPitch, Yaw;
        public short VelocityX, VelocityY, VelocityZ;
        public MetadataDictionary Metadata;

        public const byte PacketID = 0x0F;
        public byte Id { get { return PacketID; } }
    
        public void ReadPacket(PacketByteReader stream)
        {
            EntityID = stream.ReadVarInt();
            Type = (Mobs)stream.ReadByte();
            X = stream.ReadInt();
            Y = stream.ReadInt();
            Z = stream.ReadInt();
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
            stream.WriteInt(X);
            stream.WriteInt(Y);
            stream.WriteInt(Z);
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