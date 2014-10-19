using MineLib.Network.IO;

namespace MineLib.Network.Modern.Packets.Server
{
    public struct SetExperiencePacket : IPacket
    {
        public float ExperienceBar;
        public int Level;
        public int TotalExperience;

        public byte ID { get { return 0x1F; } }

        public IPacket ReadPacket(MinecraftDataReader reader)
        {
            ExperienceBar = reader.ReadFloat();
            Level = reader.ReadVarInt();
            TotalExperience = reader.ReadVarInt();

            return this;
        }

        public IPacket WritePacket(MinecraftStream stream)
        {
            stream.WriteVarInt(ID);
            stream.WriteFloat(ExperienceBar);
            stream.WriteVarInt(Level);
            stream.WriteVarInt(TotalExperience);
            stream.Purge();

            return this;
        }
    }
}