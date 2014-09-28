using MineLib.Network.IO;

namespace MineLib.Network.Packets.Server
{
    public struct EntityVelocityPacket : IPacket
    {
        public int EntityID;
        public short VelocityX, VelocityY, VelocityZ;

        public byte ID { get { return 0x12; } }

        public void ReadPacket(PacketByteReader reader)
        {
            EntityID = reader.ReadVarInt();
            VelocityX = reader.ReadShort();
            VelocityY = reader.ReadShort();
            VelocityZ = reader.ReadShort();
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(ID);
            stream.WriteVarInt(EntityID);
            stream.WriteShort(VelocityX);
            stream.WriteShort(VelocityY);
            stream.WriteShort(VelocityZ);
            stream.Purge();
        }
    }
}