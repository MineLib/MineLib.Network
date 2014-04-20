using MineLib.Network.IO;


namespace MineLib.Network.Packets.Server
{
    public struct SpawnGlobalEntityPacket : IPacket
    {
        public int EntityID;
        public byte Type;
        public double X, Y, Z;

        public const byte PacketID = 0x2C;
        public byte Id { get { return PacketID; } }

        public void ReadPacket(PacketByteReader stream)
        {
            EntityID = stream.ReadVarInt();
            Type = stream.ReadByte();
            X = stream.ReadInt() / 32;
            Y = stream.ReadInt() / 32;
            Z = stream.ReadInt() / 32;
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(Id);
            stream.WriteVarInt(EntityID);
            stream.WriteByte(Type);
            stream.WriteInt((int)X * 32);
            stream.WriteInt((int)Y * 32);
            stream.WriteInt((int)Z * 32);
            stream.Purge();
        }
    }
}