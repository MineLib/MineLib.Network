using MineLib.Network.IO;
using Org.BouncyCastle.Math;

namespace MineLib.Network.Packets.Client
{
    public struct SpectatePacket : IPacket
    {
        public BigInteger UUID;

        public const byte PacketID = 0x18;
        public byte Id { get { return PacketID; } }

        public void ReadPacket(PacketByteReader reader)
        {
            UUID = reader.ReadBigInteger();
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(Id);
            stream.WriteBigInteger(UUID);
            stream.Purge();
        }
    }
}
