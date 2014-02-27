using CWrapped;
using MineLib.Network.Enums;

namespace MineLib.Network.Packets.Server.Login
{
    public struct HandshakePacket : IPacket
    {
        public int ProtocolVersion;
        public string ServerAddress;
        public short ServerPort;
        public NextState NextState;

        public const byte PacketId = 0x00;
        public byte Id { get { return 0x00; } }

        public void ReadPacket(ref Wrapped stream)
        {
            ProtocolVersion = stream.ReadVarInt();
            ServerAddress = stream.ReadString();
            ServerPort = stream.ReadShort();
            NextState = (NextState)stream.ReadVarInt();
        }

        public void WritePacket(ref Wrapped stream)
        {
            stream.WriteVarInt(Id);
            stream.WriteVarInt(ProtocolVersion);
            stream.WriteString(ServerAddress);
            stream.WriteShort(ServerPort);
            stream.WriteVarInt((byte)NextState);
            stream.Purge();
        }
    }

}