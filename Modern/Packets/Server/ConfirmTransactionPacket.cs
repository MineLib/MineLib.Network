using MineLib.Network.IO;

namespace MineLib.Network.Modern.Packets.Server
{
    public struct ConfirmTransactionPacket : IPacket
    {
        public byte WindowId;
        public short ActionNumber;
        public bool Accepted;

        public byte ID { get { return 0x32; } }

        public void ReadPacket(PacketByteReader reader)
        {
            WindowId = reader.ReadByte();
            ActionNumber = reader.ReadShort();
            Accepted = reader.ReadBoolean();
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(ID);
            stream.WriteByte(WindowId);
            stream.WriteShort(ActionNumber);
            stream.WriteBoolean(Accepted);
            stream.Purge();
        }
    }
}