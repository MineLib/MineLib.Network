using MineLib.Network.IO;
using MineLib.Network.Packets;

namespace MineLib.Network.Classic.Packets.Client
{
    public struct PlayerIdentificationPacket : IPacket
    {
        public byte ProtocolVersion;
        public string Username;
        public string VerificationKey;
        public byte UnUsed;

        public const byte PacketID = 0x00;
        public byte ID { get { return PacketID; } }

        public void ReadPacket(PacketByteReader stream)
        {
            ProtocolVersion = stream.ReadByte();
            Username = stream.ReadString();
            VerificationKey = stream.ReadString();
            UnUsed = stream.ReadByte();
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteByte(ID);
            stream.WriteByte(ProtocolVersion);
            stream.WriteString(Username);
            stream.WriteString(VerificationKey);
            stream.WriteByte(UnUsed);
            stream.Purge();
        }
    }
}
