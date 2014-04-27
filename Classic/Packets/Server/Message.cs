using MineLib.Network.IO;
using MineLib.Network.Packets;

namespace MineLib.Network.Classic.Packets.Server
{
    public struct MessagePacket : IPacket
    {
        public sbyte PlayerID;
        public string Message;

        public const byte PacketID = 0x0D;
        public byte Id { get { return PacketID; } }

        public void ReadPacket(PacketByteReader stream)
        {
            PlayerID = stream.ReadSByte();
            Message = stream.ReadString();
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteByte(Id);
            stream.WriteSByte(PlayerID);
            stream.WriteString(Message);
            stream.Purge();
        }
    }
}
