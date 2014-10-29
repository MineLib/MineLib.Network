using MineLib.Network.IO;

namespace MineLib.Network.Classic.Packets.Extension.Server
{
    public struct ExtAddEntity2Packet : IPacketWithSize
    {
        public byte EntityID;
        public string InGameName;
        public string SkinName;
        public short SpawnX;
        public short SpawnY;
        public short SpawnZ;
        public byte SpawnYaw;
        public byte SpawnPitch;

        public byte ID { get { return 0x21; } }
        public short Size { get { return 138; } }

        public IPacketWithSize ReadPacket(IMinecraftDataReader stream)
        {
            EntityID = stream.ReadByte();
            InGameName = stream.ReadString();
            SkinName = stream.ReadString();
            SpawnX = stream.ReadShort();
            SpawnY = stream.ReadShort();
            SpawnZ = stream.ReadShort();
            SpawnYaw = stream.ReadByte();
            SpawnPitch = stream.ReadByte();

            return this;
        }

        IPacket IPacket.ReadPacket(IMinecraftDataReader stream)
        {
            return ReadPacket(stream);
        }

        public IPacket WritePacket(IMinecraftStream stream)
        {
            stream.WriteByte(ID);
            stream.WriteByte(EntityID);
            stream.WriteString(InGameName);
            stream.WriteString(SkinName);
            stream.WriteShort(SpawnX);
            stream.WriteShort(SpawnY);
            stream.WriteShort(SpawnZ);
            stream.WriteByte(SpawnYaw);
            stream.WriteByte(SpawnPitch);
            stream.Purge();

            return this;
        }
    }
}
