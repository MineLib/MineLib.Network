using MineLib.Network.Data;
using MineLib.Network.IO;

namespace MineLib.Network.Classic.Packets.Extension.Server
{
    public struct ExtAddEntity2Packet : IPacketWithSize
    {
        public byte EntityID;
        public string InGameName;
        public string SkinName;
        public Position SpawnLocation;
        public byte SpawnYaw;
        public byte SpawnPitch;

        public byte ID { get { return 0x21; } }
        public short Size { get { return 138; } }

        public IPacketWithSize ReadPacket(IMinecraftDataReader reader)
        {
            EntityID = reader.ReadByte();
            InGameName = reader.ReadString();
            SkinName = reader.ReadString();
            SpawnLocation = Position.FromReaderShort(reader);
            SpawnYaw = reader.ReadByte();
            SpawnPitch = reader.ReadByte();

            return this;
        }

        IPacket IPacket.ReadPacket(IMinecraftDataReader reader)
        {
            return ReadPacket(reader);
        }

        public IPacket WritePacket(IMinecraftStream stream)
        {
            stream.WriteByte(ID);
            stream.WriteByte(EntityID);
            stream.WriteString(InGameName);
            stream.WriteString(SkinName);
            SpawnLocation.ToStreamShort(stream);
            stream.WriteByte(SpawnYaw);
            stream.WriteByte(SpawnPitch);
            stream.Purge();

            return this;
        }
    }
}
