using MineLib.Network.IO;


namespace MineLib.Network.Packets.Server
{
    public struct EntityTeleportPacket : IPacket
    {
        public int EntityID;
        public double X, Y, Z;
        public byte Yaw, Pitch;

        public const byte PacketID = 0x18;
        public byte Id { get { return PacketID; } }

        public void ReadPacket(PacketByteReader stream)
        {
            EntityID = stream.ReadInt();
            X = stream.ReadInt() / 32;
            Y = stream.ReadInt() / 32;
            Z = stream.ReadInt() / 32;
            Yaw = stream.ReadByte();
            Pitch = stream.ReadByte();
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(Id);
            stream.WriteInt(EntityID);
            stream.WriteInt((int)X * 32);
            stream.WriteInt((int)Y * 32);
            stream.WriteInt((int)Z * 32);
            stream.WriteByte(Yaw);
            stream.WriteByte(Pitch);
            stream.Purge();
        }
    }
}