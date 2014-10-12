using MineLib.Network.IO;
using MineLib.Network.Main.Data;

namespace MineLib.Network.Main.Packets.Client
{
    public struct TabCompletePacket : IPacket
    {
        public string Text;
        public bool HasPosition;
        public Position LookedAtBlock;

        public byte ID { get { return 0x14; } }

        public void ReadPacket(PacketByteReader reader)
        {
            Text = reader.ReadString();
            HasPosition = reader.ReadBoolean();
            LookedAtBlock = Position.FromReaderLong(reader);
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(ID);
            stream.WriteString(Text);
            stream.WriteBoolean(HasPosition);
            LookedAtBlock.ToStreamLong(ref stream);
            stream.Purge();
        }
    }
}