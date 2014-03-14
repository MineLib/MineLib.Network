using CWrapped;

namespace MineLib.Network.Packets.Server
{
    public struct EntityVelocityPacket : IPacket
    {
        public int EntityID;
        public short VelocityX, VelocityY, VelocityZ;

        public const byte PacketID = 0x12;
        public byte Id { get { return PacketID; } }

        public void ReadPacket(ref Wrapped stream)
        {
            EntityID = stream.ReadInt();
            VelocityX = stream.ReadShort();
            VelocityY = stream.ReadShort();
            VelocityZ = stream.ReadShort();
        }

        public void WritePacket(ref Wrapped stream)
        {
            stream.WriteVarInt(Id);
            stream.WriteInt(EntityID);
            stream.WriteShort(VelocityX);
            stream.WriteShort(VelocityY);
            stream.WriteShort(VelocityZ);
            stream.Purge();
        }
    }
}