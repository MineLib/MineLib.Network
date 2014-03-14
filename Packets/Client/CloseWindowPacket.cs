using CWrapped;

namespace MineLib.Network.Packets.Client
{
    public struct CloseWindowPacket : IPacket
    {
        public byte WindowID;

        public const byte PacketID = 0x0D;
        public byte Id { get { return PacketID; } }

        public void ReadPacket(ref Wrapped stream)
        {
            WindowID = stream.ReadByte();
        }

        public void WritePacket(ref Wrapped stream)
        {
            stream.WriteVarInt(Id);
            stream.WriteByte(WindowID);
            stream.Purge();
        }
    }
}