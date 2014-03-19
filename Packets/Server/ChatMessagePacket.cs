using MineLib.Network.IO;


namespace MineLib.Network.Packets.Server
{
    public struct ChatMessagePacket : IPacket
    {
        public string Message;

        public const byte PacketID = 0x02;
        public byte Id { get { return PacketID; } }

        public void ReadPacket(PacketByteReader stream)
        {
            Message = stream.ReadString();
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(Id);
            stream.WriteString(Message);
            stream.Purge();
        }
    }
}