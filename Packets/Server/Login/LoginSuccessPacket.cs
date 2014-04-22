using MineLib.Network.IO;

namespace MineLib.Network.Packets.Server.Login
{
    public struct LoginSuccessPacket : IPacket
    {
        public string UUID, Username;

        public const byte PacketID = 0x02;
        public byte Id { get { return PacketID; } }

        public void ReadPacket(PacketByteReader stream)
        {
            UUID = stream.ReadString();
            Username = stream.ReadString();
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(Id);
            stream.WriteString(UUID);
            stream.WriteString(Username);
            stream.Purge();
        }
    }
}