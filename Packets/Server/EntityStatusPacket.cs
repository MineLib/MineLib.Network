using MineLib.Network.IO;
using MineLib.Network.Enums;

namespace MineLib.Network.Packets.Server
{
    public struct EntityStatusPacket : IPacket
    {
        public int EntityID;
        public EntityStatus Status;

        public const byte PacketID = 0x1A;
        public byte Id { get { return PacketID; } }

        public void ReadPacket(PacketByteReader reader)
        {
            EntityID = reader.ReadInt();
            Status = (EntityStatus) reader.ReadSByte();
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(Id);
            stream.WriteInt(EntityID);
            stream.WriteSByte((sbyte) Status);
            stream.Purge();
        }
    }
}