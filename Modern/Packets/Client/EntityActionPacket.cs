using MineLib.Network.IO;
using MineLib.Network.Modern.Enums;

namespace MineLib.Network.Modern.Packets.Client
{
    public struct EntityActionPacket : IPacket
    {
        public int EntityID;
        public EntityAction Action;

        public byte ID { get { return 0x0B; } }

        public IPacket ReadPacket(MinecraftDataReader reader)
        {
            EntityID = reader.ReadVarInt();
            Action = (EntityAction) reader.ReadByte();

            return this;
        }

        public IPacket WritePacket(MinecraftStream stream)
        {
            stream.WriteVarInt(ID);
            stream.WriteVarInt(EntityID);
            stream.WriteByte((byte) Action);
            stream.Purge();

            return this;
        }
    }
}