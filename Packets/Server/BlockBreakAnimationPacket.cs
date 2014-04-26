using MineLib.Network.Data;
using MineLib.Network.IO;

namespace MineLib.Network.Packets.Server
{
    public struct BlockBreakAnimationPacket : IPacket
    {
        public int EntityID;
        public Coordinates3D Coordinates;
        public byte DestroyStage;

        public const byte PacketID = 0x25;
        public byte Id { get { return PacketID; } }

        public void ReadPacket(PacketByteReader stream)
        {
            EntityID = stream.ReadVarInt();
            Coordinates.X = stream.ReadInt();
            Coordinates.Y = stream.ReadInt();
            Coordinates.Z = stream.ReadInt();
            DestroyStage = stream.ReadByte();
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(Id);
            stream.WriteVarInt(EntityID);
            stream.WriteInt(Coordinates.X);
            stream.WriteInt(Coordinates.Y);
            stream.WriteInt(Coordinates.Z);
            stream.WriteByte(DestroyStage);
            stream.Purge();
        }
    }
}