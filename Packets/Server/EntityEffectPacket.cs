using MineLib.Network.Enums;
using MineLib.Network.IO;

namespace MineLib.Network.Packets.Server
{
    public struct EntityEffectPacket : IPacket
    {
        public int EntityID;
        public EffectID EffectID;
        public sbyte Amplifier;
        public int Duration;
        public bool HideParticles;

        public const byte PacketID = 0x1D;
        public byte Id { get { return PacketID; } }

        public void ReadPacket(PacketByteReader reader)
        {
            EntityID = reader.ReadVarInt();
            EffectID = (EffectID) reader.ReadSByte();
            Amplifier = reader.ReadSByte();
            Duration = reader.ReadVarInt();
            HideParticles = reader.ReadBoolean();
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(Id);
            stream.WriteVarInt(EntityID);
            stream.WriteSByte((sbyte) EffectID);
            stream.WriteSByte(Amplifier);
            stream.WriteVarInt(Duration);
            stream.WriteBoolean(HideParticles);
            stream.Purge();
        }
    }
}