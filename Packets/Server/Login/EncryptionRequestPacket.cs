using System.Security.Cryptography;
using MineLib.Network.IO;

namespace MineLib.Network.Packets.Server.Login
{
    public struct EncryptionRequestPacket : IPacket
    {
        public string ServerId;
        public byte[] PublicKey;
        public byte[] SharedKey;
        public byte[] VerificationToken;

        public const byte PacketID = 0x01;
        public byte Id { get { return PacketID; } }

        public void ReadPacket(PacketByteReader reader)
        {
            ServerId = reader.ReadString();
            var pkLength = reader.ReadVarInt();
            PublicKey = reader.ReadByteArray(pkLength);
            var vtLength = reader.ReadVarInt();
            VerificationToken = reader.ReadByteArray(vtLength);

            SharedKey = new byte[16];

            var random = RandomNumberGenerator.Create();
            random.GetBytes(SharedKey);
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(Id);
            stream.WriteString(ServerId);
            stream.WriteVarInt(PublicKey.Length);
            stream.WriteByteArray(PublicKey);
            stream.WriteVarInt(VerificationToken.Length);
            stream.WriteByteArray(VerificationToken);
            stream.Purge();
        }
    }
}