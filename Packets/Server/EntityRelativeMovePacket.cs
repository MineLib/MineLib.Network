using MineLib.Network.IO;


namespace MineLib.Network.Packets.Server
{
    public struct EntityRelativeMovePacket : IPacket
    {
        public int EntityID;
        public sbyte DeltaX, DeltaY, DeltaZ;

        public const byte PacketID = 0x15;
        public byte Id { get { return PacketID; } }

        public void ReadPacket(ref Wrapped stream)
        {
            EntityID = stream.ReadInt();
            DeltaX = stream.ReadSByte();
            DeltaY = stream.ReadSByte();
            DeltaZ = stream.ReadSByte();
        }

        public void WritePacket(ref Wrapped stream)
        {
            stream.WriteVarInt(Id);
            stream.WriteInt(EntityID);
            stream.WriteSByte(DeltaX);
            stream.WriteSByte(DeltaY);
            stream.WriteSByte(DeltaZ);
            stream.Purge();
        }
    }
}