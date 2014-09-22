using MineLib.Network.IO;

namespace MineLib.Network.Packets.Server
{
    public struct EntityHeadLookPacket : IPacket
    {
        public int EntityID;
        public sbyte HeadYaw;

        public const byte PacketID = 0x19;
        public byte Id { get { return PacketID; } }

        public void ReadPacket(PacketByteReader reader)
        {
            EntityID = reader.ReadVarInt();
            HeadYaw = reader.ReadSByte();
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(Id);
            stream.WriteVarInt(EntityID);
            stream.WriteSByte(HeadYaw);
            stream.Purge();
        }
    }
}