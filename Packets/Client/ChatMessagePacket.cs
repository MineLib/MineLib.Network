using MineLib.Network.IO;

namespace MineLib.Network.Packets.Client
{
    public struct ChatMessagePacket : IPacket
    {
        public string Message;

        public byte ID { get { return 0x01; } }

        public void ReadPacket(PacketByteReader reader)
        {
            Message = reader.ReadString();
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(ID);
            stream.WriteString(Message);
            stream.Purge();
        }
    }
}