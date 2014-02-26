using CWrapped;

namespace MineLib.Network.Packets.Server
{
    public struct LoginDisconnectPacket : IPacket
    {
        public string Reason;

        public const byte PacketId = 0x00;
        public byte Id { get { return 0x00; } }

        public void ReadPacket(ref Wrapped stream)
        {
            Reason = stream.ReadString();
        }

        public void WritePacket(ref Wrapped stream)
        {
            stream.WriteVarInt(Id);
            stream.WriteString(Reason);
            stream.Purge();
        }
    }
}