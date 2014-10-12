using MineLib.Network.IO;
using MineLib.Network.Modern.Enums;

namespace MineLib.Network.Modern.Packets.Server
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

        public byte ID { get { return 0x01; } }

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
            stream.WriteVarInt(ID);
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