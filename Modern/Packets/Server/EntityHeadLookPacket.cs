using MineLib.Network.IO;

namespace MineLib.Network.Modern.Packets.Server
{
    public struct EntityHeadLookPacket : IPacket
    {
        public int EntityID;
        public sbyte HeadYaw;

        public byte ID { get { return 0x19; } }

        public IPacket ReadPacket(MinecraftDataReader reader)
        {
            EntityID = reader.ReadVarInt();
            HeadYaw = reader.ReadSByte();

            return this;
        }

        public IPacket WritePacket(MinecraftStream stream)
        {
            stream.WriteVarInt(ID);
            stream.WriteVarInt(EntityID);
            stream.WriteSByte(HeadYaw);
            stream.Purge();

            return this;
        }
    }
}