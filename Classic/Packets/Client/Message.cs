using MineLib.Network.IO;

namespace MineLib.Network.Classic.Packets.Client
{
    public struct MessagePacket : IPacketWithSize
    {
        public byte UnUsed;
        public string Message;

        public byte ID { get { return 0x0D; } }
        public short Size { get { return 66; } }

        public IPacketWithSize ReadPacket(IMinecraftDataReader reader)
        {
            UnUsed = reader.ReadByte();
            Message = reader.ReadString();

            return this;
        }

        IPacket IPacket.ReadPacket(IMinecraftDataReader reader)
        {
            return ReadPacket(reader);
        }

        public IPacket WritePacket(IMinecraftStream stream)
        {
            stream.WriteByte(ID);
            stream.WriteByte(UnUsed);
            stream.WriteString(Message);
            stream.Purge();

            return this;
        }
    }
}
