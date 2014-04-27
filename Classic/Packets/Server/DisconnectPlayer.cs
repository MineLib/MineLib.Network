using MineLib.Network.IO;
using MineLib.Network.Packets;

namespace MineLib.Network.Classic.Packets.Server
{
    public struct DisconnectPlayerPacket : IPacket
    {
        public string Reason;

        public const byte PacketID = 0x0E;
        public byte Id { get { return PacketID; } }

        public void ReadPacket(PacketByteReader stream)
        {
            Reason = stream.ReadString();
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteByte(Id);
            stream.WriteString(Reason);
            stream.Purge();
        }
    }
}
