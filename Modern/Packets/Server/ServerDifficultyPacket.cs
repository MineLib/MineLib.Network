using MineLib.Network.IO;
using MineLib.Network.Modern.Enums;

namespace MineLib.Network.Modern.Packets.Server
{
    public struct ServerDifficultyPacket : IPacket
    {
        public Difficulty Difficulty;

        public byte ID { get { return 0x41; } }

        public IPacket ReadPacket(IMinecraftDataReader reader)
        {
            Difficulty = (Difficulty) reader.ReadByte();

            return this;
        }

        public IPacket WritePacket(IMinecraftStream stream)
        {
            stream.WriteVarInt(ID);
            stream.WriteByte((byte) Difficulty);
            stream.Purge();

            return this;
        }
    }
}
