using MineLib.Network.Data;
using MineLib.Network.IO;

namespace MineLib.Network.Packets.Server
{
    public struct EntityRelativeMovePacket : IPacket
    {
        public int EntityID;
        public Vector3 DeltaVector3;

        public const byte PacketID = 0x15;
        public byte Id { get { return PacketID; } }

        public void ReadPacket(PacketByteReader stream)
        {
            EntityID = stream.ReadInt();
            DeltaVector3.X = (double)stream.ReadSByte() / 32;
            DeltaVector3.Y = (double)stream.ReadSByte() / 32;
            DeltaVector3.Z = (double)stream.ReadSByte() / 32;
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(Id);
            stream.WriteInt(EntityID);
            stream.WriteSByte((sbyte)(DeltaVector3.X * 32)); // Check that.
            stream.WriteSByte((sbyte)(DeltaVector3.Y * 32)); // Check that.
            stream.WriteSByte((sbyte)(DeltaVector3.Z * 32)); // Check that.
            stream.Purge();
        }
    }
}