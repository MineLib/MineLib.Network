using CWrapped;

namespace MineLib.Network.Packets.Server
{
    public struct EntityEffectPacket : IPacket
    {
        public int EntityID;
        public byte EffectID;
        public byte Amplifier;
        public short Duration;

        public const byte PacketId = 0x1D;
        public byte Id { get { return 0x1D; } }

        public void ReadPacket(ref Wrapped stream)
        {
            EntityID = stream.ReadInt();
            EffectID = stream.ReadByte();
            Amplifier = stream.ReadByte();
            Duration = stream.ReadShort();
        }

        public void WritePacket(ref Wrapped stream)
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