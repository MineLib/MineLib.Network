using MineLib.Network.Enums;
using MineLib.Network.IO;

namespace MineLib.Network.Packets.Server
{
    public struct ChatMessagePacket : IPacket
    {
        public string Message;
        public ChatMessagePosition Position;

        public const byte PacketID = 0x02;
        public byte Id { get { return PacketID; } }

        public void ReadPacket(PacketByteReader reader)
        {
            Message = reader.ReadString();
            Position = (ChatMessagePosition) reader.ReadByte();
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(Id);
            stream.WriteString(Message);
            stream.WriteByte((byte) Position);
            stream.Purge();
        }
    }
}