using MineLib.Network.IO;

namespace MineLib.Network.Classic.Packets.Server
{
    public struct DisconnectPlayerPacket : IPacketWithSize
    {
        public string Reason;

        public byte ID { get { return 0x0E; } }
        public short Size { get { return 65; } }

        public IPacketWithSize ReadPacket(IMinecraftDataReader stream)
        {
            Reason = stream.ReadString();

            return this;
        }

        IPacket IPacket.ReadPacket(IMinecraftDataReader stream)
        {
            return ReadPacket(stream);
        }

        public IPacket WritePacket(IMinecraftStream stream)
        {
            stream.WriteByte(ID);
            stream.WriteString(Reason);
            stream.Purge();

            return this;
        }
    }
}
