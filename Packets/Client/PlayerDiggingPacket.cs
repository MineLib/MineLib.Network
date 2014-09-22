using MineLib.Network.Data;
using MineLib.Network.IO;
using MineLib.Network.Enums;

namespace MineLib.Network.Packets.Client
{
    public struct PlayerDiggingPacket : IPacket
    {
        public BlockStatus Status;
        public Position Location;
        public byte Face;

        public const byte PacketID = 0x07;
        public byte Id { get { return PacketID; } }

        public void ReadPacket(PacketByteReader reader)
        {
            Status = (BlockStatus) reader.ReadByte();
            Location = Position.FromReaderLong(reader);
            Face = reader.ReadByte();
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(Id);
            stream.WriteByte((byte) Status);
            Location.ToStreamLong(ref stream);
            stream.WriteByte(Face);
            stream.Purge();
        }
    }
}