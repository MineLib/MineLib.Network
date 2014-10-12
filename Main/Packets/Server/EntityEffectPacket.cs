using MineLib.Network.IO;
using MineLib.Network.Main.Enums;

namespace MineLib.Network.Main.Packets.Server
{
    public struct EntityEffectPacket : IPacket
    {
        public int EntityID;
        public EffectID EffectID;
        public sbyte Amplifier;
        public int Duration;
        public bool HideParticles;

        public byte ID { get { return 0x1D; } }

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
            stream.WriteVarInt(ID);
            stream.WriteVarInt(EntityID);
            stream.WriteSByte((sbyte) EffectID);
            stream.WriteSByte(Amplifier);
            stream.WriteVarInt(Duration);
            stream.WriteBoolean(HideParticles);
            stream.Purge();
        }
    }
}