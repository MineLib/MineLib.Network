using MineLib.Network.IO;

namespace MineLib.Network.Packets.Server
{
    public struct TabCompletePacket : IPacket
    {
        public int Count;
        public string Text;

        public byte ID { get { return 0x3A; } }

        public void ReadPacket(PacketByteReader reader)
        {
            Count = reader.ReadVarInt();
            Text = reader.ReadString();
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(ID);
            stream.WriteVarInt(Count);
            stream.WriteString(Text);
            stream.Purge();
        }
    }
}