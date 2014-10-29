using MineLib.Network.IO;

namespace MineLib.Network.Modern.Packets.Client.Status
{
    public struct PingPacket : IPacket
    {
        public long Time;

        public byte ID { get { return 0x01; } }

        public IPacket ReadPacket(IMinecraftDataReader reader)
        {
            Time = reader.ReadLong();

            return this;
        }

        public IPacket WritePacket(IMinecraftStream stream)
        {
            stream.WriteVarInt(ID);
            stream.WriteLong(Time);
            stream.Purge();

            return this;
        }

    }
}
