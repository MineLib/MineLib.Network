namespace MineLib.Network
{
    public interface IPacketWithSize : IPacket
    {
        short Size { get; }
    }
}
