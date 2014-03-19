using MineLib.Network.IO;


namespace MineLib.Network.Packets.Server
{
    public struct SetExperiencePacket : IPacket
    {
        public float ExperienceBar;
        public short Level;
        public short TotalExperience;

        public const byte PacketID = 0x1F;
        public byte Id { get { return PacketID; } }

        public void ReadPacket(PacketByteReader stream)
        {
            ExperienceBar = stream.ReadFloat();
            Level = stream.ReadShort();
            TotalExperience = stream.ReadShort();
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(Id);
            stream.WriteFloat(ExperienceBar);
            stream.WriteShort(Level);
            stream.WriteShort(TotalExperience);
            stream.Purge();
        }
    }
}