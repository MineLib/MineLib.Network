using MineLib.Network.IO;

namespace MineLib.Network
{
    public interface IPacketWithSize : IPacket
    {
        new IPacketWithSize ReadPacket(MinecraftDataReader reader);
        short Size { get; }
    }
}
