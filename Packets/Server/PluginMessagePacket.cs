using MineLib.Network.IO;

namespace MineLib.Network.Packets.Server
{
    public struct PluginMessagePacket : IPacket
    {
        public string Channel;
        public byte[] Data;

        public byte ID { get { return 0x3F; } }

        public void ReadPacket(PacketByteReader reader)
        {
            Channel = reader.ReadString();
            var length = reader.ReadVarInt();
            Data = reader.ReadByteArray(length);
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(ID);
            stream.WriteString(Channel);
            stream.WriteVarInt(Data.Length);
            stream.WriteByteArray(Data);
            stream.Purge();
        }
    }
}