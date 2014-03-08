using CWrapped;

namespace MineLib.Network.Packets.Client.Status
{
    public struct ResponsePacket : IPacket
    {
        public string Response;

        public const byte PacketID = 0x00;
        public byte Id { get { return 0x00; } }

        public void ReadPacket(ref Wrapped stream)
        {
            Response = stream.ReadString();
        }

        public void WritePacket(ref Wrapped stream)
        {
            stream.WriteVarInt(Id);
            stream.WriteString(Response);
            stream.Purge();
        }

    }
}
