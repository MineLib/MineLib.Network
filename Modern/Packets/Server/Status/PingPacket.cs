using MineLib.Network.IO;

namespace MineLib.Network.Modern.Packets.Server.Status
{
    public struct PingPacket : IPacket
    {
        public long Time;

        public byte ID { get { return 0x01; } }

        public IPacket ReadPacket(MinecraftDataReader reader)
        {
            Time = reader.ReadLong();

            return this;
        }

        public IPacket WritePacket(MinecraftStream stream)
        {
            stream.WriteVarInt(ID);
            stream.WriteLong(Time);
            stream.Purge();

            return this;
        }
    }
}