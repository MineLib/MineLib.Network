using MineLib.Network.IO;

namespace MineLib.Network.Packets.Server.Login
{
    public struct ResourcePackSendPacket : IPacket
    {
        public string URL;
        public string Hash;

        public const byte PacketID = 0x48;
        public byte Id { get { return PacketID; } }

        public void ReadPacket(PacketByteReader reader)
        {
            URL = reader.ReadString();
            Hash = reader.ReadString();
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(Id);
            stream.WriteString(URL);
            stream.WriteString(Hash);
            stream.Purge();
        }
    }
}
