using MineLib.Network.IO;

namespace MineLib.Network.PocketEdition.Packets.Client
{
    public class MessagePacket : IPacketWithSize
    {
        public string Message;

        public byte ID { get { return 0x85; } }
        public short Size { get { return 0; } }

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
