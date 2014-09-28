using MineLib.Network.IO;

namespace MineLib.Network.Packets.Client
{
    public struct KeepAlivePacket : IPacket
    {
        public int KeepAlive;

        public byte ID { get { return 0x00; } }

        public void ReadPacket(PacketByteReader reader)
        {
            KeepAlive = reader.ReadVarInt();
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(ID);
            stream.WriteVarInt(KeepAlive);
            stream.Purge();
        }
    }
}