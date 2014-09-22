using MineLib.Network.IO;

namespace MineLib.Network.Packets.Server
{
    public struct UpdateEntityNBTPacket : IPacket
    {
        public int EntityID;
        public byte[] NBTTag;

        public const byte PacketID = 0x49;
        public byte Id { get { return PacketID; } }

        public void ReadPacket(PacketByteReader reader)
        {
            EntityID = reader.ReadVarInt();
            var length = reader.ReadVarInt(); // TODO: Check that
            NBTTag = reader.ReadByteArray(length);
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(Id);
            stream.WriteVarInt(EntityID);
            stream.WriteVarInt(NBTTag.Length);
            stream.WriteByteArray(NBTTag);
            stream.Purge();
        }
    }
}
