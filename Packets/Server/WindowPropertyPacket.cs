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

        public void ReadPacket(ref Wrapped stream)
        {
            WindowId = stream.ReadByte();
            PropertyId = stream.ReadShort();
            Value = stream.ReadShort();
        }

        public void WritePacket(ref Wrapped stream)
        {
            stream.WriteVarInt(Id);
            stream.WriteByte(WindowId);
            stream.WriteShort(PropertyId);
            stream.WriteShort(Value);
            stream.Purge();
        }
    }
}