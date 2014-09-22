using MineLib.Network.IO;

namespace MineLib.Network.Packets.Client.Login
{
    public struct LoginStartPacket : IPacket
    {
        public string Name;

        public const byte PacketID = 0x00;
        public byte Id { get { return PacketID; } }

        public void ReadPacket(PacketByteReader reader)
        {
            Name = reader.ReadString();
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(Id);
            stream.WriteString(Name);
            stream.Purge();
        }
    }
}