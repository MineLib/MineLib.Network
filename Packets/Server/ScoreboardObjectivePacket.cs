using CWrapped;

namespace MineLib.Network.Packets.Server
{
    public struct ScoreboardObjectivePacket : IPacket
    {
        public string ObjectiveName;
        public string ObjectiveValue;
        public byte CreateRemove;

        public const byte PacketId = 0x3B;
        public byte Id { get { return 0x3B; } }

        public void ReadPacket(ref Wrapped stream)
        {
            ObjectiveName = stream.ReadString();
            ObjectiveValue = stream.ReadString();
            CreateRemove = stream.ReadByte();
        }

        public void WritePacket(ref Wrapped stream)
        {
            stream.WriteVarInt(Id);
            stream.WriteString(ObjectiveName);
            stream.WriteString(ObjectiveValue);
            stream.WriteByte(CreateRemove);
            stream.Purge();
        }
    }
}