using MineLib.Network.IO;
using MineLib.Network.Modern.Enums;

namespace MineLib.Network.Modern.Packets.Server
{
    public struct DisplayScoreboardPacket : IPacket
    {
        public ScoreboardPosition Position;
        public string ScoreName;

        public byte ID { get { return 0x3D; } }

        public IPacket ReadPacket(MinecraftDataReader reader)
        {
            Position = (ScoreboardPosition) reader.ReadSByte();
            ScoreName = reader.ReadString();

            return this;
        }

        public IPacket WritePacket(MinecraftStream stream)
        {
            stream.WriteVarInt(ID);
            stream.WriteSByte((sbyte) Position);
            stream.WriteString(ScoreName);
            stream.Purge();

            return this;
        }
    }
}