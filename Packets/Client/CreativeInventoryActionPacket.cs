using MineLib.Network.IO;

namespace MineLib.Network.Packets.Client
{
    public struct CreativeInventoryActionPacket : IPacket
    {
        public byte WindowId;
        public short ActionNumber;
        public bool Accepted;

        public const byte PacketID = 0x10;
        public byte Id { get { return PacketID; } }

        public void ReadPacket(PacketByteReader stream)
        {
            WindowId = stream.ReadByte();
            ActionNumber = stream.ReadShort();
            Accepted = stream.ReadBool();
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(Id);
            stream.WriteByte(WindowId);
            stream.WriteShort(ActionNumber);
            stream.WriteBool(Accepted);
            stream.Purge();
        }
    }
}