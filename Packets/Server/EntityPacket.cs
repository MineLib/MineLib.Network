using MineLib.Network.IO;

namespace MineLib.Network.Packets.Server
{
    public struct EntityPacket : IPacket
    {
        public int EntityID;

        public const byte PacketID = 0x14;
        public byte Id { get { return PacketID; } }

        public void ReadPacket(PacketByteReader stream)
        {
            EntityID = stream.ReadInt();
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(Id);
            stream.WriteInt(EntityID);
            stream.Purge();
        }
    }
}