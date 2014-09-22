using MineLib.Network.IO;

namespace MineLib.Network.Packets.Client
{
    public struct ConfirmTransactionPacket : IPacket
    {
        public byte WindowID;
        public short Slot;
        public bool Accepted;

        public const byte PacketID = 0x0F;
        public byte Id { get { return PacketID; } }

        public void ReadPacket(PacketByteReader reader)
        {
            WindowID = reader.ReadByte();
            Slot = reader.ReadShort();
            Accepted = reader.ReadBoolean();
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(Id);
            stream.WriteByte(WindowID);
            stream.WriteShort(Slot);
            stream.WriteBoolean(Accepted);
            stream.Purge();
        }
    }
}