using MineLib.Network.IO;

namespace MineLib.Network.Packets.Server
{
    public struct PluginMessagePacket : IPacket
    {
        public string Channel;
        public byte[] Data;

        public const byte PacketId = 0x3F;
        public byte Id { get { return PacketId; } }

        public void ReadPacket(PacketByteReader reader)
        {
            Channel = reader.ReadString();
            var length = reader.ReadVarInt();
            Data = reader.ReadByteArray(length);
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(Id);
            stream.WriteString(Channel);
            stream.WriteVarInt(Data.Length);
            stream.WriteByteArray(Data);
            stream.Purge();
        }
    }
}