using MineLib.Network.IO;

namespace MineLib.Network.Main.Packets.Server.Status
{
    public struct ResponsePacket : IPacket
    {
        public string Response;

        public byte ID { get { return 0x00; } }

        public void ReadPacket(PacketByteReader reader)
        {
            Response = reader.ReadString();
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(ID);
            stream.WriteString(Response);
            stream.Purge();
        }
    }
}