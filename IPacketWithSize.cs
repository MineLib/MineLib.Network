using MineLib.Network.IO;

namespace MineLib.Network
{
    public interface IPacketWithSize : IPacket
    {
        new IPacketWithSize ReadPacket(IMinecraftDataReader reader);
        short Size { get; }
    }
}
