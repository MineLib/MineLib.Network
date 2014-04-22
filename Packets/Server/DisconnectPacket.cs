using MineLib.Network.IO;

namespace MineLib.Network.Packets.Server
{
    public struct DisconnectPacket : IPacket
    {
        public string Reason;

        public const byte PacketID = 0x40;
        public byte Id { get { return PacketID; } }

        public void ReadPacket(PacketByteReader stream)
        {
            Reason = stream.ReadString();
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(Id);
            stream.WriteString(Reason);
            stream.Purge();
        }
    }
}