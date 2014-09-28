using MineLib.Network.IO;

namespace MineLib.Network.Packets.Server.Login
{
    public interface ISetCompression : IPacket
    {
        int Threshold { get; set; }
    }

    public struct SetCompressionPacket : ISetCompression
    {
        public int Threshold { get; set; }

        public byte ID { get { return 0x03; } }

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
