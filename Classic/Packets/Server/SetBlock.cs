using MineLib.Network.Data;
using MineLib.Network.IO;
using MineLib.Network.Packets;

namespace MineLib.Network.Classic.Packets.Server
{
    public struct SetBlockPacket : IPacket
    {
        public Position Coordinates;
        public byte BlockType;

        public const byte PacketID = 0x06;
        public byte ID { get { return PacketID; } }

        public void ReadPacket(PacketByteReader stream)
        {
            Coordinates.X = stream.ReadShort();
            Coordinates.Y = stream.ReadShort();
            Coordinates.Z = stream.ReadShort();
            BlockType = stream.ReadByte();
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteByte(ID);
            stream.WriteShort((short)Coordinates.X);
            stream.WriteShort((short)Coordinates.Y);
            stream.WriteShort((short)Coordinates.Z);
            stream.WriteByte(BlockType);
            stream.Purge();
        }
    }
}
