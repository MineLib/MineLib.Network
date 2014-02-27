using CWrapped;

namespace MineLib.Network.Packets.Server.Login
{
    public struct LoginSuccessPacket : IPacket
    {
        public string UUID, Username;

        public const byte PacketId = 0x02;
        public byte Id { get { return 0x02; } }

        public void ReadPacket(ref Wrapped stream)
        {
            UUID = stream.ReadString();
            Username = stream.ReadString();
        }

        public void WritePacket(ref Wrapped stream)
        {
            stream.WriteVarInt(Id);
            stream.WriteString(UUID);
            stream.WriteString(Username);
            stream.Purge();
        }
    }
}