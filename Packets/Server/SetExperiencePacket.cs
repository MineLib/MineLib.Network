using MineLib.Network.IO;

namespace MineLib.Network.Packets.Server
{
    public struct SetExperiencePacket : IPacket
    {
        public float ExperienceBar;
        public int Level;
        public int TotalExperience;

        public const byte PacketID = 0x1F;
        public byte Id { get { return PacketID; } }

        public void ReadPacket(PacketByteReader reader)
        {
            ExperienceBar = reader.ReadFloat();
            Level = reader.ReadVarInt();
            TotalExperience = reader.ReadVarInt();
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(Id);
            stream.WriteFloat(ExperienceBar);
            stream.WriteVarInt(Level);
            stream.WriteVarInt(TotalExperience);
            stream.Purge();
        }
    }
}