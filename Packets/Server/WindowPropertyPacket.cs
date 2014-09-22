using MineLib.Network.IO;

namespace MineLib.Network.Packets.Server
{
    public struct WindowPropertyPacket : IPacket
    {
        public byte WindowId;
        public short PropertyId;
        public short Value;

        public const byte PacketID = 0x31;
        public byte Id { get { return PacketID; } }

        public void ReadPacket(PacketByteReader reader)
        {
            WindowId = reader.ReadByte();
            PropertyId = reader.ReadShort();
            Value = reader.ReadShort();
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(Id);
            stream.WriteByte(WindowId);
            stream.WriteShort(PropertyId);
            stream.WriteShort(Value);
            stream.Purge();
        }
    }
}