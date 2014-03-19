using MineLib.Network.IO;


namespace MineLib.Network.Packets.Server
{
    public struct RemoveEntityEffectPacket : IPacket
    {
        public int EntityID;
        public byte EffectID;

        public const byte PacketId = 0x1E;
        public byte Id { get { return PacketId; } }

        public void ReadPacket(PacketByteReader stream)
        {
            EntityID = stream.ReadInt();
            EffectID = stream.ReadByte();
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(Id);
            stream.WriteInt(EntityID);
            stream.WriteByte(EffectID);
            stream.Purge();
        }
    }
}