using MineLib.Network.Data;
using MineLib.Network.IO;
using MineLib.Network.Packets;

namespace MineLib.Network.Classic.Packets.Server
{
    public struct LevelFinalizePacket : IPacket
    {
        public Position Coordinates;

        public const byte PacketID = 0x04;
        public byte Id { get { return PacketID; } }

        public void ReadPacket(PacketByteReader stream)
        {
            Coordinates.X = stream.ReadShort();
            Coordinates.Y = stream.ReadShort();
            Coordinates.Z = stream.ReadShort();
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteByte(Id);
            stream.WriteShort((short)Coordinates.X);
            stream.WriteShort((short)Coordinates.Y);
            stream.WriteShort((short)Coordinates.Z);
            stream.Purge();
        }
    }
}
