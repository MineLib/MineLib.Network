using MineLib.Network.Data;
using MineLib.Network.IO;

namespace MineLib.Network.Classic.Packets.Server
{
    public struct PositionUpdatePacket : IPacketWithSize
    {
        public sbyte PlayerID;
        public Position ChangeLocation;

        public byte ID { get { return 0x0A; } }
        public short Size { get { return 5; } }

        public IPacketWithSize ReadPacket(IMinecraftDataReader reader)
        {
            PlayerID = reader.ReadSByte();
            ChangeLocation = Position.FromReaderSByte(reader);

            return this;
        }

        IPacket IPacket.ReadPacket(IMinecraftDataReader reader)
        {
            return ReadPacket(reader);
        }

        public IPacket WritePacket(IMinecraftStream stream)
        {
            stream.WriteByte(ID);
            stream.WriteSByte(PlayerID);
            ChangeLocation.ToStreamSByte(stream);
            stream.Purge();

            return this;
        }
    }
}
