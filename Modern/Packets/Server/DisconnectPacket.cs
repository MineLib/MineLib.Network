using MineLib.Network.IO;

namespace MineLib.Network.Modern.Packets.Server
{
    public struct DisconnectPacket : IPacket
    {
        public string Reason;

        public byte ID { get { return 0x40; } }

        public void ReadPacket(PacketByteReader reader)
        {
            Reason = reader.ReadString();
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(ID);
            stream.WriteString(Reason);
            stream.Purge();
        }
    }
}