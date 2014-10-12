using MineLib.Network.IO;

namespace MineLib.Network.Main.Packets.Server
{
    public struct UpdateEntityNBTPacket : IPacket
    {
        public int EntityID;
        public byte[] NBTTag;

        public byte ID { get { return 0x49; } }

        public void ReadPacket(PacketByteReader reader)
        {
            EntityID = reader.ReadVarInt();
            var length = reader.ReadVarInt(); // TODO: Check that
            NBTTag = reader.ReadByteArray(length);
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(ID);
            stream.WriteVarInt(EntityID);
            stream.WriteVarInt(NBTTag.Length);
            stream.WriteByteArray(NBTTag);
            stream.Purge();
        }
    }
}
