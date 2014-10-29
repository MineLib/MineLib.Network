using MineLib.Network.IO;
using MineLib.Network.Modern.Enums;

namespace MineLib.Network.Modern.Packets.Server
{
    public struct AnimationPacket : IPacket
    {
        public int EntityID;
        public Animation Animation;

        public byte ID { get { return 0x0B; } }

        public IPacket ReadPacket(IMinecraftDataReader reader)
        {
            EntityID = reader.ReadVarInt();
            Animation = (Animation) reader.ReadByte();

            return this;
        }

        public IPacket WritePacket(IMinecraftStream stream)
        {
            stream.WriteVarInt(ID);
            stream.WriteVarInt(EntityID);
            stream.WriteByte((byte) Animation);
            stream.Purge();

            return this;
        }
    }
}