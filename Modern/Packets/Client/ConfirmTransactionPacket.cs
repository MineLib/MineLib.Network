using MineLib.Network.IO;

namespace MineLib.Network.Modern.Packets.Client
{
    public struct ConfirmTransactionPacket : IPacket
    {
        public byte WindowID;
        public short Slot;
        public bool Accepted;

        public byte ID { get { return 0x0F; } }

        public void ReadPacket(PacketByteReader reader)
        {
            WindowID = reader.ReadByte();
            Slot = reader.ReadShort();
            Accepted = reader.ReadBoolean();
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(ID);
            stream.WriteByte(WindowID);
            stream.WriteShort(Slot);
            stream.WriteBoolean(Accepted);
            stream.Purge();
        }
    }
}