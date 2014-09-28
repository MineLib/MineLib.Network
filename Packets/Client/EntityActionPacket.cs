using MineLib.Network.IO;
using MineLib.Network.Enums;

namespace MineLib.Network.Packets.Client
{
    public struct EntityActionPacket : IPacket
    {
        public int EntityID;
        public EntityAction Action;

        public byte ID { get { return 0x0B; } }

        public void ReadPacket(PacketByteReader reader)
        {
            EntityID = reader.ReadVarInt();
            Action = (EntityAction) reader.ReadByte();
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(ID);
            stream.WriteVarInt(EntityID);
            stream.WriteByte((byte) Action);
            stream.Purge();
        }
    }
}