using MineLib.Network.IO;

namespace MineLib.Network.Packets.Server.Login
{
    public struct LoginDisconnectPacket : IPacket
    {
        public string Reason;

        public byte ID { get { return 0x00; } }

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