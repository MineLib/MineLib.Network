using MineLib.Network.IO;

namespace MineLib.Network.Classic.Packets.Extension.Server
{
    public struct ChangeModelPacket : IPacketWithSize
    {
        public byte EntityID;
        public string ModelName;

        public byte ID { get { return 0x1D; } }
        public short Size { get { return 66; } }

        public IPacketWithSize ReadPacket(IMinecraftDataReader stream)
        {
            EntityID = stream.ReadByte();
            ModelName = stream.ReadString();

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
            stream.WriteString(ModelName);
            stream.Purge();

            return this;
        }
    }
}
