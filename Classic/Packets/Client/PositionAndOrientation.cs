using MineLib.Network.Data;
using MineLib.Network.IO;

namespace MineLib.Network.Classic.Packets.Client
{
    public struct PositionAndOrientationPacket : IPacketWithSize
    {
        public byte PlayerID;
        public Position Coordinates;
        public byte Yaw;
        public byte Pitch;

        public byte ID { get { return 0x08; } }
        public short Size { get { return 10; } }

        public IPacketWithSize ReadPacket(IMinecraftDataReader reader)
        {
            PlayerID = reader.ReadByte();
            Coordinates = Position.FromReaderShort(reader);
            Yaw = reader.ReadByte();
            Pitch = reader.ReadByte();

            return this;
        }

        IPacket IPacket.ReadPacket(IMinecraftDataReader reader)
        {
            return ReadPacket(reader);
        }

        public IPacket WritePacket(IMinecraftStream stream)
        {
            stream.WriteByte(ID);
            stream.WriteByte(PlayerID);
            Coordinates.ToStreamShort(stream);
            stream.WriteByte(Yaw);
            stream.WriteByte(Pitch);
            stream.Purge();

            return this;
        }
    }
}
