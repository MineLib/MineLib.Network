using CWrapped;

namespace MineLib.Network.Packets.Client
{
    public struct TabCompletePacket : IPacket
    {
        public string Text;

        public const byte PacketId = 0x14;
        public byte Id { get { return 0x14; } }

        public void ReadPacket(ref Wrapped stream)
        {
            Text = stream.ReadString();
        }

        public void WritePacket(ref Wrapped stream)
        {
            stream.WriteVarInt(Id);
            stream.WriteString(Text);
            stream.Purge();
        }
    }
}