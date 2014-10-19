using MineLib.Network.IO;

namespace MineLib.Network.Modern.Packets.Server
{
    public struct EntityLookPacket : IPacket
    {
        public int EntityID;
        public sbyte Yaw;
        public sbyte Pitch;
        public bool OnGround;

        public byte ID { get { return 0x16; } }

        public IPacket ReadPacket(MinecraftDataReader reader)
        {
            EntityID = reader.ReadVarInt();
            Yaw = reader.ReadSByte();
            Pitch = reader.ReadSByte();
            OnGround = reader.ReadBoolean();

            return this;
        }

        public IPacket WritePacket(MinecraftStream stream)
        {
            stream.WriteVarInt(ID);
            stream.WriteVarInt(EntityID);
            stream.WriteSByte(Yaw);
            stream.WriteSByte(Pitch);
            stream.WriteBoolean(OnGround);
            stream.Purge();

            return this;
        }
    }
}