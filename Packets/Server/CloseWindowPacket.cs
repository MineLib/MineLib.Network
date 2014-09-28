using MineLib.Network.IO;

namespace MineLib.Network.Packets.Server
{
    public struct CloseWindowPacket : IPacket
    {
        public byte WindowID;

        public byte ID { get { return 0x2E; } }

        public void ReadPacket(PacketByteReader reader)
        {
            WindowID = reader.ReadByte();
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(ID);
            stream.WriteByte(WindowID);
            stream.Purge();
        }
    }
}