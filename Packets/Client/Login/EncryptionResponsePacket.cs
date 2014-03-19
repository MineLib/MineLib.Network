using MineLib.Network.IO;


namespace MineLib.Network.Packets.Client.Login
{
    public struct EncryptionResponsePacket : IPacket
    {
        public byte[] SharedSecret;
        public byte[] VerificationToken;

        public const byte PacketID = 0x01;
        public byte Id { get { return PacketID; } }

        public void ReadPacket(PacketByteReader stream)
        {
            var ssLength = stream.ReadShort();
            SharedSecret = stream.ReadByteArray(ssLength);
            var vtLength = stream.ReadShort();
            VerificationToken = stream.ReadByteArray(vtLength);
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(Id);
            stream.WriteShort((short)SharedSecret.Length);
            stream.WriteByteArray(SharedSecret);
            stream.WriteShort((short)VerificationToken.Length);
            stream.WriteByteArray(VerificationToken);
            stream.Purge();
        }
    }
}