using MineLib.Network.IO;
using MineLib.Network.Packets.Server.Login;

namespace MineLib.Network.Packets.Server
{
    public struct SetCompressionPacket : ISetCompression
    {
        public int Threshold { get; set; }

        public byte ID { get { return 0x46; } }

        public void ReadPacket(PacketByteReader reader)
        {
            Threshold = reader.ReadVarInt();
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(ID);
            stream.WriteVarInt(Threshold);
            stream.Purge();
        }
    }
}
