using MineLib.Network.IO;
using MineLib.Network.Main.Data;
using Org.BouncyCastle.Math;

namespace MineLib.Network.Main.Packets.Server
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
    
        public void ReadPacket(PacketByteReader reader)
        {
            EntityID = reader.ReadVarInt();
            PlayerUUID = reader.ReadBigInteger();
            Vector3 = Vector3.FromReaderIntFixedPoint(reader);
            Yaw = reader.ReadSByte();
            Pitch = reader.ReadSByte();
            CurrentItem = reader.ReadShort();
            EntityMetadata = EntityMetadata.FromReader(reader);
        }
    
        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(ID);
            stream.WriteVarInt(EntityID);
            stream.WriteBigInteger(PlayerUUID);
            Vector3.ToStreamIntFixedPoint(ref stream);
            stream.WriteSByte(Yaw);
            stream.WriteSByte(Pitch);
            stream.WriteShort(CurrentItem);
            EntityMetadata.ToStream(ref stream);
            stream.Purge();
        }
    }
}