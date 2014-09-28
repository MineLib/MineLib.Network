using MineLib.Network.IO;
using MineLib.Network.Enums;

namespace MineLib.Network.Packets.Server
{
    public struct AnimationPacket : IPacket
    {
        public int EntityID;
        public Animation Animation;

        public byte ID { get { return 0x0B; } }

        public void ReadPacket(PacketByteReader reader)
        {
            EntityID = reader.ReadVarInt();
            Animation = (Animation) reader.ReadByte();
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(ID);
            stream.WriteVarInt(EntityID);
            stream.WriteByte((byte) Animation);
            stream.Purge();
        }
    }
}