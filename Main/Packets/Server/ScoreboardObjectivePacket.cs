using MineLib.Network.IO;

namespace MineLib.Network.Main.Packets.Server
{
    public struct ScoreboardObjectivePacket : IPacket
    {
        public string ObjectiveName;
        public sbyte Mode;
        public string ObjectiveValue;
        public string Type;

        public byte ID { get { return 0x3B; } }

        public void ReadPacket(PacketByteReader reader)
        {
            ObjectiveName = reader.ReadString();
            Mode = reader.ReadSByte();
            ObjectiveValue = reader.ReadString();
            Type = reader.ReadString();
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(ID);
            stream.WriteString(ObjectiveName);
            stream.WriteSByte(Mode);
            stream.WriteString(ObjectiveValue);
            stream.WriteString(Type);
            stream.Purge();
        }
    }
}