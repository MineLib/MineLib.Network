using MineLib.Network.IO;

namespace MineLib.Network.Packets.Client
{
    public struct AnimationPacket : IPacket
    {
        public const byte PacketID = 0x0A;
        public byte Id { get { return PacketID; } }

        public void ReadPacket(PacketByteReader reader)
        {
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(Id);
            stream.Purge();
        }
    }
}