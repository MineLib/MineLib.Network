using MineLib.Network.Data;
using MineLib.Network.IO;

namespace MineLib.Network.Packets.Server
{
    public struct SpawnPaintingPacket : IPacket
    {
        public int EntityID;
        public string Title;
        public Vector3 Vector3;
        public int Direction;

        public const byte PacketID = 0x10;
        public byte Id { get { return PacketID; } }

        public void ReadPacket(PacketByteReader stream)
        {
            EntityID = stream.ReadVarInt();
            Title = stream.ReadString();
            Vector3.X = stream.ReadInt();
            Vector3.Y = stream.ReadInt();
            Vector3.Z = stream.ReadInt();
            Direction = stream.ReadInt();
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(Id);
            stream.WriteVarInt(EntityID);
            stream.WriteString(Title);
            stream.WriteInt((int)Vector3.X);
            stream.WriteInt((int)Vector3.Y);
            stream.WriteInt((int)Vector3.Z);
            stream.WriteInt(Direction);
            stream.Purge();
        }
    }
}