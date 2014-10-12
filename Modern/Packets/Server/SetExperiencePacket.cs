using MineLib.Network.IO;

namespace MineLib.Network.Modern.Packets.Server
{
    public struct SetExperiencePacket : IPacket
    {
        public float ExperienceBar;
        public int Level;
        public int TotalExperience;

        public byte ID { get { return 0x1F; } }

        public void ReadPacket(PacketByteReader reader)
        {
            ExperienceBar = reader.ReadFloat();
            Level = reader.ReadVarInt();
            TotalExperience = reader.ReadVarInt();
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(ID);
            stream.WriteFloat(ExperienceBar);
            stream.WriteVarInt(Level);
            stream.WriteVarInt(TotalExperience);
            stream.Purge();
        }
    }
}