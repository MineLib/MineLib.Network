using MineLib.Network.IO;

namespace MineLib.Network.Modern.Packets.Client.Status
{
    public struct RequestPacket : IPacket
    {
        public byte ID { get { return 0x00; } }

        public IPacket ReadPacket(MinecraftDataReader reader)
        {
            return this;
        }

        public IPacket WritePacket(MinecraftStream stream)
        {
            stream.WriteVarInt(ID);
            stream.Purge();

            return this;
        }
    }
}
