using MineLib.Network.IO;

namespace MineLib.Network.Modern.Packets.Server
{
    public struct PluginMessagePacket : IPacket
    {
        public string Channel;
        public byte[] Data;

        public byte ID { get { return 0x3F; } }

        public IPacket ReadPacket(MinecraftDataReader reader)
        {
            Channel = reader.ReadString();
            var length = reader.ReadVarInt();
            Data = reader.ReadByteArray(length);

            return this;
        }

        public IPacket WritePacket(MinecraftStream stream)
        {
            stream.WriteVarInt(ID);
            stream.WriteString(Channel);
            stream.WriteVarInt(Data.Length);
            stream.WriteByteArray(Data);
            stream.Purge();

            return this;
        }
    }
}