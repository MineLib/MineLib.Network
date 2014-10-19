using MineLib.Network.IO;

namespace MineLib.Network.Modern.Packets.Client.Login
{
    public struct LoginStartPacket : IPacket
    {
        public string Name;

        public byte ID { get { return 0x00; } }

        public IPacket ReadPacket(MinecraftDataReader reader)
        {
            Name = reader.ReadString();

            return this;
        }

        public IPacket WritePacket(MinecraftStream stream)
        {
            stream.WriteVarInt(ID);
            stream.WriteString(Name);
            stream.Purge();

            return this;
        }
    }
}