using MineLib.Network.IO;

namespace MineLib.Network.PocketEdition.Packets.Client
{
    public class StartGamePacket : IPacketWithSize
    {
        public int LevelSeed;
        public int Unknown;
        public int Gamemode;
        public int EntityID;
        public float X;
        public float Y;
        public float Z;

        public byte ID { get { return 0x82; } }
        public short Size { get { return 0; } }

        public void ReadPacket(PacketByteReader reader)
        {
            LevelSeed = reader.ReadInt();
            Unknown = reader.ReadInt();
            Gamemode = reader.ReadInt();
            EntityID = reader.ReadInt();
            X = reader.ReadFloat();
            Y = reader.ReadFloat();
            Z = reader.ReadFloat();
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(ID);
            stream.WriteInt(LevelSeed);
            stream.WriteInt(Unknown);
            stream.WriteInt(Gamemode);
            stream.WriteInt(EntityID);
            stream.WriteFloat(X);
            stream.WriteFloat(Y);
            stream.WriteFloat(Z);
            stream.Purge();
        }
    }
}
