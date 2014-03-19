using MineLib.Network.IO;


namespace MineLib.Network.Packets.Server
{
    public struct SpawnPositionPacket : IPacket
    {
        public int X, Y, Z;

        public const byte PacketID = 0x05;
        public byte Id { get { return PacketID; } }

        public void ReadPacket(PacketByteReader stream)
        {
            X = stream.ReadInt();
            Y = stream.ReadInt();
            Z = stream.ReadInt();
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(Id);
            stream.WriteInt(X);
            stream.WriteInt(Y);
            stream.WriteInt(Z);
            stream.Purge();
        }
    }
}