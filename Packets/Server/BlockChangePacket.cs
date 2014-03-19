using MineLib.Network.IO;


namespace MineLib.Network.Packets.Server
{
    public struct BlockChangePacket : IPacket
    {
        public int X;
        public byte Y;
        public int Z;
        public int BlockID;
        public byte BlockMetadata;

        public const byte PacketID = 0x23;
        public byte Id { get { return PacketID; } }

        public void ReadPacket(PacketByteReader stream)
        {
            X = stream.ReadInt();
            Y = stream.ReadByte();
            Z = stream.ReadInt();
            BlockID = stream.ReadVarInt();
            BlockMetadata = stream.ReadByte();
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(Id);
            stream.WriteInt(X);
            stream.WriteByte(Y);
            stream.WriteInt(Z);
            stream.WriteVarInt(BlockID);
            stream.WriteByte(BlockMetadata);
            stream.Purge();
        }
    }
}