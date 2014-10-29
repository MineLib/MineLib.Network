using MineLib.Network.IO;

namespace MineLib.Network
{
    public interface IPacket
    {
        byte ID { get; }
        IPacket ReadPacket(IMinecraftDataReader reader);
        IPacket WritePacket(IMinecraftStream stream);
    }
}
