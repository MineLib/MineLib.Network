using MineLib.Network.IO;
using MineLib.Network.Modern.Packets.Server.Login;

namespace MineLib.Network.Modern.Packets.Server
{
    public struct SetCompressionPacket : ISetCompression
    {
        public int Threshold { get; set; }

        public byte ID { get { return 0x46; } }

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
