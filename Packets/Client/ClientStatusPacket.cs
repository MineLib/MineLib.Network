using MineLib.Network.IO;
using MineLib.Network.Enums;

namespace MineLib.Network.Packets.Client
{
    public struct ClientStatusPacket : IPacket
    {
        public ClientStatus Status;

        public const byte PacketID = 0x16;
        public byte Id { get { return PacketID; } }

        public void ReadPacket(PacketByteReader reader)
        {
            Status = (ClientStatus) reader.ReadByte();
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(Id);
            stream.WriteByte((byte) Status);
            stream.Purge();
        }
    }
}