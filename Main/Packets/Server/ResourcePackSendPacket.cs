using MineLib.Network.IO;

namespace MineLib.Network.Main.Packets.Server
{
    public struct ResourcePackSendPacket : IPacket
    {
        public string URL;
        public string Hash;

        public byte ID { get { return 0x48; } }

        public void ReadPacket(PacketByteReader reader)
        {
            URL = reader.ReadString();
            Hash = reader.ReadString();
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(ID);
            stream.WriteString(URL);
            stream.WriteString(Hash);
            stream.Purge();
        }
    }
}
