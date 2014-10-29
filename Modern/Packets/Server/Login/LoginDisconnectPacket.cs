using MineLib.Network.IO;

namespace MineLib.Network.Modern.Packets.Server.Login
{
    public struct LoginDisconnectPacket : IPacket
    {
        public string Reason;

        public byte ID { get { return 0x00; } }

        public IPacket ReadPacket(IMinecraftDataReader reader)
        {
            Reason = reader.ReadString();

            return this;
        }

        public IPacket WritePacket(IMinecraftStream stream)
        {
            stream.WriteVarInt(ID);
            stream.WriteString(Reason);
            stream.Purge();

            return this;
        }
    }
}