using MineLib.Network.Data;
using MineLib.Network.IO;
using Org.BouncyCastle.Math;

namespace MineLib.Network.Packets.Server
{
    public struct SpawnPlayerPacket : IPacket
    {
        public int EntityID;
        public BigInteger PlayerUUID;
        public Vector3 Vector3;
        public sbyte Yaw, Pitch;
        public short CurrentItem;
        public EntityMetadata Metadata;

        public const byte PacketID = 0x0C;
        public byte Id { get { return PacketID; } }
    
        public void ReadPacket(PacketByteReader reader)
        {
            EntityID = reader.ReadVarInt();
            PlayerUUID = reader.ReadBigInteger();
            Vector3 = Vector3.FromReaderIntFixedPoint(reader);
            Yaw = reader.ReadSByte();
            Pitch = reader.ReadSByte();
            CurrentItem = reader.ReadShort();
            Metadata = EntityMetadata.FromReader(reader);
        }
    
        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(Id);
            stream.WriteVarInt(EntityID);
            stream.WriteBigInteger(PlayerUUID);
            Vector3.ToStreamIntFixedPoint(ref stream);
            stream.WriteSByte(Yaw);
            stream.WriteSByte(Pitch);
            stream.WriteShort(CurrentItem);
            Metadata.ToStream(ref stream);
            stream.Purge();
        }
    }
}