using MineLib.Network.IO;

namespace MineLib.Network.Modern.Packets.Client.Status
{
    public struct RequestPacket : IPacket
    {
        public byte ID { get { return 0x00; } }

        public void ReadPacket(PacketByteReader reader)
        {
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(ID);
            stream.Purge();
        }

    }
}
