using MineLib.Network.IO;
using MineLib.Network.Modern.Data;

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

        public IPacketWithSize ReadPacket(IMinecraftDataReader stream)
        {
            PlayerID = stream.ReadSByte();
            PlayerName = stream.ReadString();
            Coordinates.X = stream.ReadShort();
            Coordinates.Y = stream.ReadShort();
            Coordinates.Z = stream.ReadShort();
            Yaw = stream.ReadByte();
            Pitch = stream.ReadByte();

            return this;
        }

        IPacket IPacket.ReadPacket(IMinecraftDataReader stream)
        {
            return ReadPacket(stream);
        }

        public IPacket WritePacket(IMinecraftStream stream)
        {
            stream.WriteByte(ID);
            stream.WriteSByte(PlayerID);
            stream.WriteString(PlayerName);
            stream.WriteShort((short)Coordinates.X);
            stream.WriteShort((short)Coordinates.Y);
            stream.WriteShort((short)Coordinates.Z);
            stream.WriteByte(Yaw);
            stream.WriteByte(Pitch);
            stream.Purge();

            return this;
        }
    }
}
