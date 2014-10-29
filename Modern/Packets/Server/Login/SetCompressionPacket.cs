using MineLib.Network.IO;

namespace MineLib.Network.Modern.Packets.Server.Login
{
    public interface ISetCompression : IPacket
    {
        int Threshold { get; set; }
    }

    public struct SetCompressionPacket : ISetCompression
    {
        public int Threshold { get; set; }

        public byte ID { get { return 0x03; } }

        public IPacket ReadPacket(IMinecraftDataReader reader)
        {
            Threshold = reader.ReadVarInt();

            return this;
        }

        public IPacket WritePacket(IMinecraftStream stream)
        {
            stream.WriteVarInt(ID);
            stream.WriteVarInt(Threshold);
            stream.Purge();

            return this;
        }
    }
}
