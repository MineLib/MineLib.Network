using MineLib.Network.IO;

namespace MineLib.Network.Modern.Packets.Client.Login
{
    public struct EncryptionResponsePacket : IPacket
    {
        public byte[] SharedSecret;
        public byte[] VerificationToken;

        public byte ID { get { return 0x01; } }

        public IPacket ReadPacket(IMinecraftDataReader reader)
        {
            var ssLength = reader.ReadVarInt();
            SharedSecret = reader.ReadByteArray(ssLength);
            var vtLength = reader.ReadVarInt();
            VerificationToken = reader.ReadByteArray(vtLength);

            return this;
        }

        public IPacket WritePacket(IMinecraftStream stream)
        {
            stream.WriteVarInt(ID);
            stream.WriteVarInt(SharedSecret.Length);
            stream.WriteByteArray(SharedSecret);
            stream.WriteVarInt(VerificationToken.Length);
            stream.WriteByteArray(VerificationToken);
            stream.Purge();

            return this;
        }
    }
}