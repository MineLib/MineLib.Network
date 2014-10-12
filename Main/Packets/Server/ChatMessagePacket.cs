using MineLib.Network.IO;
using MineLib.Network.Main.Enums;

namespace MineLib.Network.Main.Packets.Server
{
    public struct ChatMessagePacket : IPacket
    {
        public string Message;
        public ChatMessagePosition Position;

        public byte ID { get { return 0x02; } }

        public void ReadPacket(PacketByteReader reader)
        {
            Message = reader.ReadString();
            Position = (ChatMessagePosition) reader.ReadByte();
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(ID);
            stream.WriteString(Message);
            stream.WriteByte((byte) Position);
            stream.Purge();
        }
    }
}