using MineLib.Network.IO;

namespace MineLib.Network.Classic.Packets.Client
{
    public struct PlayerIdentificationPacket : IPacketWithSize
    {
        public byte ProtocolVersion;
        public string Username;
        public string VerificationKey;
        public byte UnUsed;

        public byte ID { get { return 0x00; } }
        public short Size { get { return 131; } }

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
