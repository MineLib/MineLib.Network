using MineLib.Network.IO;
using MineLib.Network.Modern.Enums;

namespace MineLib.Network.Modern.Packets.Server
{
    public struct ChatMessagePacket : IPacket
    {
        public string Message;
        public ChatMessagePosition Position;

        public byte ID { get { return 0x02; } }

        public IPacket ReadPacket(MinecraftDataReader reader)
        {
            Message = reader.ReadString();
            Position = (ChatMessagePosition) reader.ReadByte();

            return this;
        }

        public IPacket WritePacket(MinecraftStream stream)
        {
            stream.WriteVarInt(ID);
            stream.WriteString(Message);
            stream.WriteByte((byte) Position);
            stream.Purge();

            return this;
        }
    }
}