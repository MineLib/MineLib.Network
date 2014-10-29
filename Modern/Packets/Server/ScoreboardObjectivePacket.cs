using MineLib.Network.IO;

namespace MineLib.Network.Modern.Packets.Server
{
    public struct ScoreboardObjectivePacket : IPacket
    {
        public string ObjectiveName;
        public sbyte Mode;
        public string ObjectiveValue;
        public string Type;

        public byte ID { get { return 0x3B; } }

        public IPacket ReadPacket(IMinecraftDataReader reader)
        {
            ObjectiveName = reader.ReadString();
            Mode = reader.ReadSByte();
            ObjectiveValue = reader.ReadString();
            Type = reader.ReadString();

            return this;
        }

        public IPacket WritePacket(IMinecraftStream stream)
        {
            stream.WriteVarInt(ID);
            stream.WriteString(ObjectiveName);
            stream.WriteSByte(Mode);
            stream.WriteString(ObjectiveValue);
            stream.WriteString(Type);
            stream.Purge();

            return this;
        }
    }
}