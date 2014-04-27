using MineLib.Network.IO;
using MineLib.Network.Packets;

namespace MineLib.Network.Classic.Packets.Server
{
    public struct LevelInitializePacket : IPacket
    {
        public const byte PacketID = 0x02;
        public byte Id { get { return PacketID; } }

        public void ReadPacket(PacketByteReader stream)
        {
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteByte(Id);
            stream.Purge();
        }
    }
}
