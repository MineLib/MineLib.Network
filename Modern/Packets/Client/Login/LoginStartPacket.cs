using MineLib.Network.IO;

namespace MineLib.Network.Modern.Packets.Client.Login
{
    public struct LoginStartPacket : IPacket
    {
        public string Name;

        public byte ID { get { return 0x00; } }

        public void ReadPacket(PacketByteReader reader)
        {
            Name = reader.ReadString();
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(ID);
            stream.WriteString(Name);
            stream.Purge();
        }
    }
}