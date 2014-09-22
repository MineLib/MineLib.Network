using MineLib.Network.IO;

namespace MineLib.Network.Packets.Server.Login
{
    public struct LoginSuccessPacket : IPacket
    {
        public string UUID;
        public string Username;

        public const byte PacketID = 0x02;
        public byte Id { get { return PacketID; } }

        public void ReadPacket(PacketByteReader reader)
        {
            UUID = reader.ReadString();
            Username = reader.ReadString();
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