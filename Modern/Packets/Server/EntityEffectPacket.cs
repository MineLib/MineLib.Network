using MineLib.Network.IO;
using MineLib.Network.Modern.Enums;

namespace MineLib.Network.Modern.Packets.Server
{
    public struct EntityEffectPacket : IPacket
    {
        public int EntityID;
        public EffectID EffectID;
        public sbyte Amplifier;
        public int Duration;
        public bool HideParticles;

        public byte ID { get { return 0x1D; } }

        public IPacket ReadPacket(IMinecraftDataReader reader)
        {
            EntityID = reader.ReadVarInt();
            EffectID = (EffectID) reader.ReadSByte();
            Amplifier = reader.ReadSByte();
            Duration = reader.ReadVarInt();
            HideParticles = reader.ReadBoolean();

            return this;
        }

        public IPacket WritePacket(IMinecraftStream stream)
        {
            stream.WriteVarInt(ID);
            stream.WriteVarInt(EntityID);
            stream.WriteSByte((sbyte) EffectID);
            stream.WriteSByte(Amplifier);
            stream.WriteVarInt(Duration);
            stream.WriteBoolean(HideParticles);
            stream.Purge();

            return this;
        }
    }
}