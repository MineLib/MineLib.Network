using MineLib.Network.IO;
using MineLib.Network.Modern.Data;

namespace MineLib.Network.Classic.Packets.Server
{
    public struct LevelFinalizePacket : IPacketWithSize
    {
        public Position Coordinates;

        public byte ID { get { return 0x04; } }
        public short Size { get { return 7; } }

        public IPacketWithSize ReadPacket(MinecraftDataReader stream)
        {
            Coordinates.X = stream.ReadShort();
            Coordinates.Y = stream.ReadShort();
            Coordinates.Z = stream.ReadShort();

            return this;
        }

        IPacket IPacket.ReadPacket(MinecraftDataReader stream)
        {
            return ReadPacket(stream);
        }

        public IPacket WritePacket(MinecraftStream stream)
        {
            stream.WriteByte(ID);
            stream.WriteShort((short)Coordinates.X);
            stream.WriteShort((short)Coordinates.Y);
            stream.WriteShort((short)Coordinates.Z);
            stream.Purge();

            return this;
        }
    }
}
