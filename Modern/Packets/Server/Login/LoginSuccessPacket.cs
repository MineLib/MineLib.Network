using MineLib.Network.IO;

namespace MineLib.Network.Modern.Packets.Server.Login
{
    public struct LoginSuccessPacket : IPacket
    {
        public string UUID;
        public string Username;

        public byte ID { get { return 0x02; } }

        public IPacket ReadPacket(IMinecraftDataReader reader)
        {
            UUID = reader.ReadString();
            Username = reader.ReadString();

            return this;
        }

        public IPacket WritePacket(IMinecraftStream stream)
        {
            stream.WriteVarInt(ID);
            stream.WriteString(UUID);
            stream.WriteString(Username);
            stream.Purge();

            return this;
        }
    }
}