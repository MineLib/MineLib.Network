using MineLib.Network.Classic.Enums;
using MineLib.Network.IO;

namespace MineLib.Network.Classic.Packets.Extension.Server
{
    public struct EnvSetWeatherTypePacket : IPacketWithSize
    {
        public WeatherType WeatherType;

        public byte ID { get { return 0x1F; } }
        public short Size { get { return 2; } }

        public IPacketWithSize ReadPacket(IMinecraftDataReader reader)
        {
            WeatherType = (WeatherType) reader.ReadByte();

            return this;
        }

        IPacket IPacket.ReadPacket(IMinecraftDataReader reader)
        {
            return ReadPacket(reader);
        }

        public IPacket WritePacket(IMinecraftStream stream)
        {
            stream.WriteByte(ID);
            stream.WriteByte((byte) WeatherType);
            stream.Purge();

            return this;
        }
    }
}
