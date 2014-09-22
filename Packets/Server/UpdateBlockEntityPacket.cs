using MineLib.Network.Data;
using MineLib.Network.Enums;
using MineLib.Network.IO;

namespace MineLib.Network.Packets.Server
{
    public struct UpdateBlockEntityPacket : IPacket
    {
        public Position Location;
        public UpdateBlockEntityAction Action;
        public byte[] NBTData;

        public const byte PacketID = 0x35;
        public byte Id { get { return PacketID; } }

        public void ReadPacket(PacketByteReader reader)
        {
            Location = Position.FromReaderLong(reader);
            Action = (UpdateBlockEntityAction) reader.ReadByte();
            int length = reader.ReadVarInt();
            NBTData = reader.ReadByteArray(length);
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(Id);
            Location.ToStreamLong(ref stream);
            stream.WriteByte((byte) Action);
            stream.WriteVarInt(NBTData.Length);
            stream.WriteByteArray(NBTData);
            stream.Purge();
        }
    }
}
