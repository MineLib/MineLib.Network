using MineLib.Network.IO;
using MineLib.Network.Modern.Data;

namespace MineLib.Network.Modern.Packets.Client
{
    public struct TabCompletePacket : IPacket
    {
        public string Text;
        public bool HasPosition;
        public Position LookedAtBlock;

        public byte ID { get { return 0x14; } }

        public IPacket ReadPacket(MinecraftDataReader reader)
        {
            Text = reader.ReadString();
            HasPosition = reader.ReadBoolean();
            LookedAtBlock = Position.FromReaderLong(reader);

            return this;
        }

        public IPacket WritePacket(MinecraftStream stream)
        {
            stream.WriteVarInt(ID);
            stream.WriteString(Text);
            stream.WriteBoolean(HasPosition);
            LookedAtBlock.ToStreamLong(stream);
            stream.Purge();

            return this;
        }
    }
}