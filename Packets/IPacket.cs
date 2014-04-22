using MineLib.Network.IO;

namespace MineLib.Network.Packets
{
    public interface IPacket
    {
        byte Id { get; }
        void ReadPacket(PacketByteReader stream);
        void WritePacket(ref PacketStream stream);
    }
}
