using MineLib.Network.IO;


namespace MineLib.Network.Packets.Server
{
    public struct PluginMessagePacket : IPacket
    {
        public string Channel;
        public byte[] Data;

        public const byte PacketId = 0x3F;
        public byte Id { get { return PacketId; } }

        public void ReadPacket(ref Wrapped stream)
        {
            Channel = stream.ReadString();
            var length = stream.ReadShort();
            Data = stream.ReadByteArray(length);
        }

        public void WritePacket(ref Wrapped stream)
        {
            stream.WriteVarInt(Id);
            stream.WriteString(Channel);
            stream.WriteShort((short)Data.Length);
            stream.WriteByteArray(Data);
            stream.Purge();
        }
    }
}