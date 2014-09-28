using MineLib.Network.IO;
using MineLib.Network.Enums;

namespace MineLib.Network.Packets.Server
{
    public struct DisplayScoreboardPacket : IPacket
    {
        public ScoreboardPosition Position;
        public string ScoreName;

        public byte ID { get { return 0x3D; } }

        public void ReadPacket(PacketByteReader reader)
        {
            Position = (ScoreboardPosition) reader.ReadSByte();
            ScoreName = reader.ReadString();
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(ID);
            stream.WriteSByte((sbyte) Position);
            stream.WriteString(ScoreName);
            stream.Purge();
        }
    }
}