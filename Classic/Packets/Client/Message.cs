using MineLib.Network.IO;
using MineLib.Network.Packets;

namespace MineLib.Network.Classic.Packets.Client
{
    public struct MessagePacket : IPacket
    {
        public byte UnUsed;
        public string Message;

        public const byte PacketID = 0x0D;
        public byte ID { get { return PacketID; } }

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
