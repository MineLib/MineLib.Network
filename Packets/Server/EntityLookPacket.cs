using MineLib.Network.IO;


namespace MineLib.Network.Packets.Server
{
    public struct EntityLookPacket : IPacket
    {
        public int EntityID;
        public byte Yaw, Pitch;

        public const byte PacketID = 0x16;
        public byte Id { get { return PacketID; } }

        public void ReadPacket(ref Wrapped stream)
        {
            EntityID = stream.ReadInt();
            Yaw = stream.ReadByte();
            Pitch = stream.ReadByte();
        }

        public void WritePacket(ref Wrapped stream)
        {
            stream.WriteVarInt(Id);
            stream.WriteInt(EntityID);
            stream.WriteByte(Yaw);
            stream.WriteByte(Pitch);
            stream.Purge();
        }
    }
}