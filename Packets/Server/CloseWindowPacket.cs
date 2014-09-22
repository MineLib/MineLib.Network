using MineLib.Network.IO;

namespace MineLib.Network.Packets.Server
{
    public struct CloseWindowPacket : IPacket
    {
        public byte WindowID;

        public const byte PacketID = 0x2E;
        public byte Id { get { return PacketID; } }

        public void ReadPacket(PacketByteReader reader)
        {
            WindowID = reader.ReadByte();
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(Id);
            stream.WriteByte(WindowID);
            stream.Purge();
        }
    }
}