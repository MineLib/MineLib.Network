using MineLib.Network.IO;

namespace MineLib.Network.Modern.Packets.Client
{
    public struct ChatMessagePacket : IPacket
    {
        public string Message;

        public byte ID { get { return 0x01; } }

        public IPacket ReadPacket(MinecraftDataReader reader)
        {
            Message = reader.ReadString();
            return this;
        }

        public IPacket WritePacket(MinecraftStream stream)
        {
            stream.WriteVarInt(ID);
            stream.WriteString(Message);
            stream.Purge();

            return this;
        }
    }
}