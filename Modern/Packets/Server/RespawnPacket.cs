using MineLib.Network.IO;
using MineLib.Network.Modern.Enums;

namespace MineLib.Network.Modern.Packets.Server
{
    public struct RespawnPacket : IPacket
    {
        public Dimension Dimension;
        public Difficulty Difficulty;
        public GameMode GameMode;
        public string LevelType;
    
        public byte ID { get { return 0x07; } }

        public IPacket ReadPacket(MinecraftDataReader reader)
        {
            Dimension = (Dimension) reader.ReadInt();
            Difficulty = (Difficulty) reader.ReadByte();
            GameMode = (GameMode) reader.ReadByte();
            LevelType = reader.ReadString();

            return this;
        }
    
        public IPacket WritePacket(MinecraftStream stream)
        {
            stream.WriteVarInt(ID);
            stream.WriteInt((int) Dimension);
            stream.WriteByte((byte) Difficulty);
            stream.WriteByte((byte) GameMode);
            stream.WriteString(LevelType);
            stream.Purge();

            return this;
        }
    }
}