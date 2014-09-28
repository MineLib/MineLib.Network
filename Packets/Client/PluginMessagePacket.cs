using MineLib.Network.IO;

namespace MineLib.Network.Packets.Client
{
    public struct PluginMessagePacket : IPacket
    {
        public string Channel;
        public byte[] Data;

        public byte ID { get { return 0x17; } }

        public void ReadPacket(PacketByteReader reader)
        {
            Channel = reader.ReadString();
            int length = reader.ReadShort();
            Data = reader.ReadByteArray(length);
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(ID);
            stream.WriteString(Channel);
            stream.WriteShort((short) Data.Length);
            stream.WriteByteArray(Data);
            stream.Purge();
        }
    }
}