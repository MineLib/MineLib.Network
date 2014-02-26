using CWrapped;

namespace MineLib.Network.Packets.Server
{
    public struct EntityLookAndRelativeMovePacket : IPacket
    {
        public int EntityID;
        public sbyte DeltaX, DeltaY, DeltaZ;
        public byte Yaw, Pitch;

        public const byte PacketId = 0x17;
        public byte Id { get { return 0x17; } }

        public void ReadPacket(ref Wrapped stream)
        {
            EntityID = stream.ReadInt();
            DeltaX = stream.ReadSByte();
            DeltaY = stream.ReadSByte();
            DeltaZ = stream.ReadSByte();
            Yaw = stream.ReadByte();
            Pitch = stream.ReadByte();
        }

        public void WritePacket(ref Wrapped stream)
        {
            stream.WriteVarInt(Id);
            stream.WriteInt(EntityID);
            stream.WriteSByte(DeltaX);
            stream.WriteSByte(DeltaY);
            stream.WriteSByte(DeltaZ);
            stream.WriteByte(Yaw);
            stream.WriteByte(Pitch);
            stream.Purge();
        }
    }
}