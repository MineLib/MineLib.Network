using MineLib.Network.IO;

namespace MineLib.Network.Packets.Server
{
    public struct EntityLookPacket : IPacket
    {
        public int EntityID;
        public byte Yaw;
        public byte Pitch;

        public const byte PacketID = 0x16;
        public byte Id { get { return PacketID; } }

        public void ReadPacket(PacketByteReader stream)
        {
            EntityID = stream.ReadInt();
            Yaw = stream.ReadByte();
            Pitch = stream.ReadByte();
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(Id);
            stream.WriteInt(EntityID);
            stream.WriteByte(Yaw);
            stream.WriteByte(Pitch);
            stream.Purge();
        }
    }
}