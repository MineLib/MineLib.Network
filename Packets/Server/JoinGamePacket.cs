using MineLib.Network.IO;
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

        public const byte PacketID = 0x01;
        public byte Id { get { return PacketID; } }
    
        public void ReadPacket(PacketByteReader stream)
        {
            EntityID = stream.ReadInt();
            GameMode = (GameMode)stream.ReadByte();
            Dimension = (Dimension)stream.ReadSByte();
            Difficulty = (Difficulty)stream.ReadByte();
            MaxPlayers = stream.ReadByte();
            LevelType = stream.ReadString();
        }
    
        public void WritePacket(ref PacketStream stream)
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