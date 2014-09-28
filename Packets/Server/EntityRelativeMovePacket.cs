using MineLib.Network.Data;
using MineLib.Network.IO;

namespace MineLib.Network.Packets.Server
{
    public struct EntityRelativeMovePacket : IPacket
    {
        public int EntityID;
        public Vector3 DeltaVector3;
        public bool OnGround;

        public byte ID { get { return 0x15; } }

        public void ReadPacket(PacketByteReader reader)
        {
            EntityID = reader.ReadVarInt();
            DeltaVector3 = Vector3.FromReaderSByteFixedPoint(reader);
            OnGround = reader.ReadBoolean();
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(ID);
            stream.WriteVarInt(EntityID);
            DeltaVector3.ToStreamSByteFixedPoint(ref stream);
            stream.WriteBoolean(OnGround);
            stream.Purge();
        }
    }
}