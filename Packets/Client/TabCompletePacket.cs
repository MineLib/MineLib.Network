using MineLib.Network.IO;


namespace MineLib.Network.Packets.Client
{
    public struct TabCompletePacket : IPacket
    {
        public string Text;

        public const byte PacketID = 0x14;
        public byte Id { get { return PacketID; } }

        public void ReadPacket(PacketByteReader stream)
        {
            Text = stream.ReadString();
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(Id);
            stream.WriteString(Text);
            stream.Purge();
        }
    }
}