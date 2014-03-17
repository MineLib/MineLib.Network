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
    
        public void ReadPacket(ref Wrapped stream)
        {
            Dimension = (Dimension)stream.ReadInt();
            Difficulty = (Difficulty)stream.ReadByte();
            GameMode = (GameMode)stream.ReadByte();
            LevelType = stream.ReadString();
        }
    
        public void WritePacket(ref Wrapped stream)
        {
            stream.WriteVarInt(Id);
            stream.WriteVarInt((int)Dimension);
            stream.WriteByte((byte)Difficulty);
            stream.WriteByte((byte)GameMode);
            stream.WriteString(LevelType);
            stream.Purge();
        }
    }
}