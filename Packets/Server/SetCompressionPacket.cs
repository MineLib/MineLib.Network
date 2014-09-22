using MineLib.Network.IO;

namespace MineLib.Network.Packets.Server
{
    public struct SetCompressionPacket : IPacket
    {
        public int Threshold;

        public const byte PacketID = 0x46;
        public byte Id { get { return PacketID; } }

        public void ReadPacket(PacketByteReader reader)
        {
            Threshold = reader.ReadVarInt();
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(Id);
            stream.WriteVarInt(Threshold);
            stream.Purge();
        }
    }
}
