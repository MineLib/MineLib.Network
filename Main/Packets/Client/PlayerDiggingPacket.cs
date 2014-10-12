using MineLib.Network.IO;
using MineLib.Network.Main.Data;
using MineLib.Network.Main.Enums;

namespace MineLib.Network.Main.Packets.Client
{
    public struct PlayerDiggingPacket : IPacket
    {
        public BlockStatus Status;
        public Position Location;
        public byte Face;

        public byte ID { get { return 0x07; } }

        public void ReadPacket(PacketByteReader reader)
        {
            Status = (BlockStatus) reader.ReadByte();
            Location = Position.FromReaderLong(reader);
            Face = reader.ReadByte();
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(ID);
            stream.WriteByte((byte) Status);
            Location.ToStreamLong(ref stream);
            stream.WriteByte(Face);
            stream.Purge();
        }
    }
}