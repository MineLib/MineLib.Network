using MineLib.Network.IO;

namespace MineLib.Network.Modern.Packets.Server
{
    public struct EntityVelocityPacket : IPacket
    {
        public int EntityID;
        public short VelocityX, VelocityY, VelocityZ;

        public byte ID { get { return 0x12; } }

        public IPacket ReadPacket(IMinecraftDataReader reader)
        {
            EntityID = reader.ReadVarInt();
            VelocityX = reader.ReadShort();
            VelocityY = reader.ReadShort();
            VelocityZ = reader.ReadShort();

            return this;
        }

        public IPacket WritePacket(IMinecraftStream stream)
        {
            stream.WriteVarInt(ID);
            stream.WriteVarInt(EntityID);
            stream.WriteShort(VelocityX);
            stream.WriteShort(VelocityY);
            stream.WriteShort(VelocityZ);
            stream.Purge();

            return this;
        }
    }
}