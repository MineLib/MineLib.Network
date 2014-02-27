using CWrapped;

namespace MineLib.Network.Packets.Client
{
    public struct ChatMessagePacket : IPacket
    {
        public string Message;

        public const byte PacketID = 0x01;
        public byte Id { get { return 0x01; } }

        public void ReadPacket(ref Wrapped stream)
        {
            Message = stream.ReadString();
        }

        public void WritePacket(ref Wrapped stream)
        {
            stream.WriteVarInt(Id);
            stream.WriteString(Message);
            stream.Purge();
        }
    }
}