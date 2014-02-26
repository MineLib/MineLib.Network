using CWrapped;

namespace MineLib.Network.Packets.Client
{
    public struct EncryptionKeyResponsePacket : IPacket
    {
        public byte[] SharedSecret;
        public byte[] VerificationToken;

        public const byte PacketId = 0xFC;
        public byte Id { get { return 0xFC; } }

        public void ReadPacket(ref Wrapped stream)
        {
            var ssLength = stream.ReadShort();
            //SharedSecret = stream.ReadUInt8Array(ssLength);
            var vtLength = stream.ReadShort();
            //VerificationToken = stream.ReadUInt8Array(vtLength);
        }

        public void WritePacket(ref Wrapped stream)
        {
            stream.WriteVarInt(Id);
            stream.WriteShort((short)SharedSecret.Length);
            //stream.writeVarIntArray(SharedSecret);
            stream.WriteShort((short)VerificationToken.Length);
            //stream.writeVarIntArray(VerificationToken);
            stream.Purge();
        }
    }
}