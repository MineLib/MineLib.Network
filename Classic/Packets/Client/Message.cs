using MineLib.Network.IO;

namespace MineLib.Network.Classic.Packets.Client
{
    public struct MessagePacket : IPacketWithSize
    {
        public byte UnUsed;
        public string Message;

        public byte ID { get { return 0x0D; } }
        public short Size { get { return 66; } }

        public void ReadPacket(PacketByteReader stream)
        {
            UnUsed = stream.ReadByte();
            Message = stream.ReadString();
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteByte(ID);
            stream.WriteByte(UnUsed);
            stream.WriteString(Message);
            stream.Purge();
        }
    }
}
