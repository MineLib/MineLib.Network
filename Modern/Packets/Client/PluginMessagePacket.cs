using MineLib.Network.IO;

namespace MineLib.Network.Modern.Packets.Client
{
    public struct PluginMessagePacket : IPacket
    {
        public string Channel;
        public byte[] Data;

        public byte ID { get { return 0x17; } }

        public IPacket ReadPacket(IMinecraftDataReader reader)
        {
            Channel = reader.ReadString();
            int length = reader.ReadShort();
            Data = reader.ReadByteArray(length);

            return this;
        }

        public IPacket WritePacket(IMinecraftStream stream)
        {
            stream.WriteVarInt(ID);
            stream.WriteString(Channel);
            stream.WriteShort((short) Data.Length);
            stream.WriteByteArray(Data);
            stream.Purge();

            return this;
        }
    }
}