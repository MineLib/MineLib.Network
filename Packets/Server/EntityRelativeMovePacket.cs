using MineLib.Network.Data;
using MineLib.Network.IO;

namespace MineLib.Network.Packets.Server
{
    public struct EntityRelativeMovePacket : IPacket
    {
        public int EntityID;
        public double DeltaX, DeltaY, DeltaZ;

        public const byte PacketID = 0x15;
        public byte Id { get { return PacketID; } }

        public void ReadPacket(PacketByteReader stream)
        {
            EntityID = stream.ReadInt();
            DeltaX = (double)stream.ReadSByte() / 32;
            DeltaY = (double)stream.ReadSByte() / 32;
            DeltaZ = (double)stream.ReadSByte() / 32;
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(Id);
            stream.WriteInt(EntityID);
            stream.WriteSByte((sbyte)(DeltaX * 32)); // Check that.
            stream.WriteSByte((sbyte)(DeltaY * 32)); // Check that.
            stream.WriteSByte((sbyte)(DeltaZ * 32)); // Check that.
            stream.Purge();
        }
    }
}