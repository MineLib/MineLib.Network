using MineLib.Network.IO;
using MineLib.Network.Enums;


namespace MineLib.Network.Packets.Server
{
    public struct DisplayScoreboardPacket : IPacket
    {
        public ScoreboardPosition Position;
        public string ScoreName;

        public const byte PacketID = 0x3D;
        public byte Id { get { return PacketID; } }

        public void ReadPacket(PacketByteReader stream)
        {
            Position = (ScoreboardPosition)stream.ReadByte();
            ScoreName = stream.ReadString();
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(Id);
            stream.WriteByte((byte)Position);
            stream.WriteString(ScoreName);
            stream.Purge();
        }
    }
}