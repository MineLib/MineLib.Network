using MineLib.Network.IO;


namespace MineLib.Network.Packets.Server
{
    public struct SignEditorOpenPacket : IPacket
    {
        public int X, Y, Z;

        public const byte PacketID = 0x36;
        public byte Id { get { return PacketID; } }

        public void ReadPacket(ref Wrapped stream)
        {
            X = stream.ReadInt();
            Y = stream.ReadInt();
            Z = stream.ReadInt();
        }

        public void WritePacket(ref Wrapped stream)
        {
            stream.WriteVarInt(Id);
            stream.WriteInt(X);
            stream.WriteInt(Y);
            stream.WriteInt(Z);
            stream.Purge();
        }
    }
}