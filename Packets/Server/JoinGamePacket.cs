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
        public bool ReducedDebugInfo;

        public const byte PacketID = 0x01;
        public byte Id { get { return PacketID; } }

        public void ReadPacket(PacketByteReader reader)
        {
            EntityID = reader.ReadInt();
            GameMode = (GameMode) reader.ReadByte();
            Dimension = (Dimension) reader.ReadSByte();
            Difficulty = (Difficulty) reader.ReadByte();
            MaxPlayers = reader.ReadByte();
            LevelType = reader.ReadString();
            ReducedDebugInfo = reader.ReadBoolean();
        }
    
        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(Id);
            stream.WriteInt(EntityID);
            stream.WriteVarInt((byte) GameMode);
            stream.WriteSByte((sbyte) Dimension);
            stream.WriteVarInt((byte) Difficulty);
            stream.WriteByte(MaxPlayers);
            stream.WriteString(LevelType);
            stream.WriteBoolean(ReducedDebugInfo);
            stream.Purge();
        }
    }
}