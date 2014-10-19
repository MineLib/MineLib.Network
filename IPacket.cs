using MineLib.Network.IO;

namespace MineLib.Network
{
    public interface IPacket
    {
        byte ID { get; }
        IPacket ReadPacket(MinecraftDataReader reader);
        IPacket WritePacket(MinecraftStream stream);
    }
}
