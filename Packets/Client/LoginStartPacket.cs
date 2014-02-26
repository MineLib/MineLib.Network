using CWrapped;

namespace MineLib.Network.Packets.Client
{
    public struct LoginStartPacket : IPacket
    {
        public string Name;

        public const byte PacketId = 0x00;
        public byte Id { get { return 0x00; } }

        public void ReadPacket(ref Wrapped stream)
        {
            Name = stream.ReadString();
        }

        public void WritePacket(ref Wrapped stream)
        {
            stream.WriteVarInt(Id);
            stream.WriteString(Name);
            stream.Purge();
        }
    }
}