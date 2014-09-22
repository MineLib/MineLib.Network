using MineLib.Network.Data;
using MineLib.Network.IO;

namespace MineLib.Network.Packets.Client
{
    public struct TabCompletePacket : IPacket
    {
        public string Text;
        public bool HasPosition;
        public Position LookedAtBlock;

        public const byte PacketID = 0x14;
        public byte Id { get { return PacketID; } }

        public void ReadPacket(PacketByteReader reader)
        {
            Text = reader.ReadString();
            HasPosition = reader.ReadBoolean();
            LookedAtBlock = Position.FromReaderLong(reader);
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(Id);
            stream.WriteString(Text);
            stream.WriteBoolean(HasPosition);
            LookedAtBlock.ToStreamLong(ref stream);
            stream.Purge();
        }
    }
}