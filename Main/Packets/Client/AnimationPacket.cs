using MineLib.Network.IO;

namespace MineLib.Network.Main.Packets.Client
{
    public struct AnimationPacket : IPacket
    {
        public byte ID { get { return 0x0A; } }

        public void ReadPacket(PacketByteReader reader)
        {
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(ID);
            stream.Purge();
        }
    }
}