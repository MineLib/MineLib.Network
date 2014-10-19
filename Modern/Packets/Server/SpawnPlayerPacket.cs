using MineLib.Network.IO;
using MineLib.Network.Modern.Data;
using Org.BouncyCastle.Math;

namespace MineLib.Network.Modern.Packets.Server
{
    public struct SpawnPlayerPacket : IPacket
    {
        public int EntityID;
        public BigInteger PlayerUUID;
        public Vector3 Vector3;
        public sbyte Yaw, Pitch;
        public short CurrentItem;
        public EntityMetadata EntityMetadata;

        public byte ID { get { return 0x0C; } }
    
        public IPacket ReadPacket(MinecraftDataReader reader)
        {
            EntityID = reader.ReadVarInt();
            PlayerUUID = reader.ReadBigInteger();
            Vector3 = Vector3.FromReaderIntFixedPoint(reader);
            Yaw = reader.ReadSByte();
            Pitch = reader.ReadSByte();
            CurrentItem = reader.ReadShort();
            EntityMetadata = EntityMetadata.FromReader(reader);

            return this;
        }
    
        public IPacket WritePacket(MinecraftStream stream)
        {
            stream.WriteVarInt(ID);
            stream.WriteVarInt(EntityID);
            stream.WriteBigInteger(PlayerUUID);
            Vector3.ToStreamIntFixedPoint(stream);
            stream.WriteSByte(Yaw);
            stream.WriteSByte(Pitch);
            stream.WriteShort(CurrentItem);
            EntityMetadata.ToStream(stream);
            stream.Purge();

            return this;
        }
    }
}