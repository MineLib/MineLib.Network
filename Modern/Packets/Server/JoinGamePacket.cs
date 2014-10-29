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

        public IPacket ReadPacket(IMinecraftDataReader reader)
        {
            EntityID = reader.ReadInt();
            GameMode = (GameMode) reader.ReadByte();
            Dimension = (Dimension) reader.ReadSByte();
            Difficulty = (Difficulty) reader.ReadByte();
            MaxPlayers = reader.ReadByte();
            LevelType = reader.ReadString();
            ReducedDebugInfo = reader.ReadBoolean();

            return this;
        }
    
        public IPacket WritePacket(IMinecraftStream stream)
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

            return this;
        }
    }
}