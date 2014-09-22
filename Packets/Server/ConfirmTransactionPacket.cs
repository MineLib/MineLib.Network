using MineLib.Network.IO;

namespace MineLib.Network.Packets.Server
{
    public struct ConfirmTransactionPacket : IPacket
    {
        public byte WindowId;
        public short ActionNumber;
        public bool Accepted;

        public const byte PacketID = 0x32;
        public byte Id { get { return PacketID; } }

        public void ReadPacket(PacketByteReader reader)
        {
            WindowId = reader.ReadByte();
            ActionNumber = reader.ReadShort();
            Accepted = reader.ReadBoolean();
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(Id);
            stream.WriteByte(WindowId);
            stream.WriteShort(ActionNumber);
            stream.WriteBoolean(Accepted);
            stream.Purge();
        }
    }
}