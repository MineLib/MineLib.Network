using MineLib.Network.IO;

namespace MineLib.Network.Modern.Packets.Server
{
    public struct TabCompletePacket : IPacket
    {
        public int Count;
        public string Text;

        public byte ID { get { return 0x3A; } }

        public IPacket ReadPacket(MinecraftDataReader reader)
        {
            Count = reader.ReadVarInt();
            Text = reader.ReadString();

            return this;
        }

        public IPacket WritePacket(MinecraftStream stream)
        {
            stream.WriteVarInt(ID);
            stream.WriteVarInt(Count);
            stream.WriteString(Text);
            stream.Purge();

            return this;
        }
    }
}