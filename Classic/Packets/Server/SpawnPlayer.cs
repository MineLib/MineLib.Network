using MineLib.Network.Data;
using MineLib.Network.IO;

namespace MineLib.Network.Classic.Packets.Server
{
    public struct SpawnPlayerPacket : IPacketWithSize
    {
        public sbyte PlayerID;
        public string PlayerName;
        public Position Coordinates;
        public byte Yaw;
        public byte Pitch;

        public byte ID { get { return 0x07; } }
        public short Size { get { return 74; } }

        public IPacketWithSize ReadPacket(IMinecraftDataReader reader)
        {
            PlayerID = reader.ReadSByte();
            PlayerName = reader.ReadString();
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
            stream.WriteSByte(PlayerID);
            stream.WriteString(PlayerName);
            Coordinates.ToStreamShort(stream);
            stream.WriteByte(Yaw);
            stream.WriteByte(Pitch);
            stream.Purge();

            return this;
        }
    }
}
