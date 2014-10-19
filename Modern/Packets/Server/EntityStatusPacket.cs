using MineLib.Network.IO;
using MineLib.Network.Modern.Enums;

namespace MineLib.Network.Modern.Packets.Server
{
    public struct EntityStatusPacket : IPacket
    {
        public int EntityID;
        public EntityStatus Status;

        public byte ID { get { return 0x1A; } }

        public IPacket ReadPacket(MinecraftDataReader reader)
        {
            EntityID = reader.ReadInt();
            Status = (EntityStatus) reader.ReadSByte();

            return this;
        }

        public IPacket WritePacket(MinecraftStream stream)
        {
            stream.WriteVarInt(ID);
            stream.WriteInt(EntityID);
            stream.WriteSByte((sbyte) Status);
            stream.Purge();

            return this;
        }
    }
}