using MineLib.Network.IO;

namespace MineLib.Network.Classic.Packets.Server
{
    public struct DisconnectPlayerPacket : IPacketWithSize
    {
        public string Reason;

        public byte ID { get { return 0x0E; } }
        public short Size { get { return 65; } }

        public void ReadPacket(PacketByteReader stream)
        {
            Reason = stream.ReadString();
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteByte(ID);
            stream.WriteString(Reason);
            stream.Purge();
        }
    }
}
