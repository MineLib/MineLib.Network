using MineLib.Network.IO;
using MineLib.Network.Modern.Data;
using MineLib.Network.Modern.Enums;

namespace MineLib.Network.Modern.Packets.Server
{
    public struct UpdateBlockEntityPacket : IPacket
    {
        public Position Location;
        public UpdateBlockEntityAction Action;
        public byte[] NBTData;

        public byte ID { get { return 0x35; } }

        public void ReadPacket(PacketByteReader reader)
        {
            Location = Position.FromReaderLong(reader);
            Action = (UpdateBlockEntityAction) reader.ReadByte();
            int length = reader.ReadVarInt();
            NBTData = reader.ReadByteArray(length);
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(ID);
            Location.ToStreamLong(ref stream);
            stream.WriteByte((byte) Action);
            stream.WriteVarInt(NBTData.Length);
            stream.WriteByteArray(NBTData);
            stream.Purge();
        }
    }
}
