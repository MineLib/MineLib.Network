using MineLib.Network.IO;
using Org.BouncyCastle.Math;

namespace MineLib.Network.Main.Packets.Client
{
    public struct SpectatePacket : IPacket
    {
        public BigInteger UUID;

        public byte ID { get { return 0x18; } }

        public void ReadPacket(PacketByteReader reader)
        {
            UUID = reader.ReadBigInteger();
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(ID);
            stream.WriteBigInteger(UUID);
            stream.Purge();
        }
    }
}
