using MineLib.Network.IO;


namespace MineLib.Network.Packets.Server
{
    public struct EntityEffectPacket : IPacket
    {
        public int EntityID;
        public byte EffectID;
        public byte Amplifier;
        public short Duration;

        public const byte PacketID = 0x1D;
        public byte Id { get { return PacketID; } }

        public void ReadPacket(PacketByteReader stream)
        {
            EntityID = stream.ReadInt();
            EffectID = stream.ReadByte();
            Amplifier = stream.ReadByte();
            Duration = stream.ReadShort();
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(Id);
            stream.WriteInt(EntityID);
            stream.WriteByte(EffectID);
            stream.WriteByte(Amplifier);
            stream.WriteShort(Duration);
            stream.Purge();
        }
    }
}