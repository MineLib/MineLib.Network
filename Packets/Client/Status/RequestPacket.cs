using CWrapped;

namespace MineLib.Network.Packets.Client.Status
{
    public struct RequestPacket : IPacket
    {
        public const byte PacketID = 0x00;
        public byte Id { get { return 0x00; } }

        public void ReadPacket(ref Wrapped stream)
        {
        }

        public void WritePacket(ref Wrapped stream)
        {
            stream.WriteVarInt(Id);
            stream.Purge();
        }

    }
}
