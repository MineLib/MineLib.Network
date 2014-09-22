using MineLib.Network.IO;

namespace MineLib.Network.Packets.Server
{
    public struct KeepAlivePacket : IPacket
    {
        public int KeepAlive;

        public const byte PacketID = 0x00;
        public byte Id { get { return PacketID; } }

        public void ReadPacket(PacketByteReader reader)
        {
            KeepAlive = reader.ReadVarInt();
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(Id);
            stream.WriteVarInt(KeepAlive);
            stream.Purge();
        }
    }
}