using MineLib.Network.IO;

namespace MineLib.Network.Packets.Server.Login
{
    public struct LoginSuccessPacket : IPacket
    {
        public string UUID;
        public string Username;

        public byte ID { get { return 0x02; } }

        public void ReadPacket(PacketByteReader reader)
        {
            UUID = reader.ReadString();
            Username = reader.ReadString();
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(ID);
            stream.WriteString(UUID);
            stream.WriteString(Username);
            stream.Purge();
        }
    }
}