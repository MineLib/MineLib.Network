using MineLib.Network.Data;
using MineLib.Network.IO;

namespace MineLib.Network.Packets.Server
{
    public struct UseBedPacket : IPacket
    {
        public int EntityID;
        public Coordinates3D Coordinates;

        public const byte PacketID = 0x0A;
        public byte Id { get { return PacketID; } }

        public void ReadPacket(PacketByteReader stream)
        {
            EntityID = stream.ReadInt();
            Coordinates.X = stream.ReadInt();
            Coordinates.Y = stream.ReadByte();
            Coordinates.Z = stream.ReadInt();
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(Id);
            stream.WriteInt(EntityID);
            stream.WriteInt(Coordinates.X);
            stream.WriteByte((byte)Coordinates.Y);
            stream.WriteInt(Coordinates.Z);
            stream.Purge();
        }
    }
}