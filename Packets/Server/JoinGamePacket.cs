using CWrapped;
using MineLib.Network.Enums;

namespace MineLib.Network.Packets.Server
{
    public struct JoinGamePacket : IPacket
    {
        public int EntityID;
        public GameMode GameMode;
        public Dimension Dimension;
        public Difficulty Difficulty;
        public byte MaxPlayers;
        public string LevelType;
    
        public const byte PacketId = 0x01;
        public byte Id { get { return 0x01; } }
    
        public void ReadPacket(ref Wrapped stream)
        {
            EntityID = stream.ReadInt();
            GameMode = (GameMode)stream.ReadByte();
            Dimension = (Dimension)stream.ReadSByte();
            Difficulty = (Difficulty)stream.ReadByte();
            MaxPlayers = stream.ReadByte();
            LevelType = stream.ReadString();
        }
    
        public void WritePacket(ref Wrapped stream)
        {
            stream.WriteVarInt(Id);
            stream.WriteInt(EntityID);
            stream.WriteVarInt((byte)GameMode);
            stream.WriteSByte((sbyte)Dimension);
            stream.WriteVarInt((byte)Difficulty);
            stream.WriteByte(MaxPlayers);
            stream.WriteString(LevelType);
            stream.Purge();
        }
    }
}