using MineLib.Network.IO;
using MineLib.Network.Enums;

namespace MineLib.Network.Packets.Server
{
    public struct RespawnPacket : IPacket
    {
        public Dimension Dimension;
        public Difficulty Difficulty;
        public GameMode GameMode;
        public string LevelType;
    
        public const byte PacketId = 0x07;
        public byte Id { get { return PacketId; } }

        public void ReadPacket(PacketByteReader reader)
        {
            Dimension = (Dimension) reader.ReadInt();
            Difficulty = (Difficulty) reader.ReadByte();
            GameMode = (GameMode) reader.ReadByte();
            LevelType = reader.ReadString();
        }
    
        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(Id);
            stream.WriteInt((int) Dimension);
            stream.WriteByte((byte) Difficulty);
            stream.WriteByte((byte) GameMode);
            stream.WriteString(LevelType);
            stream.Purge();
        }
    }
}