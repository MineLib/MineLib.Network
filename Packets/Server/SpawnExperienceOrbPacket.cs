using MineLib.Network.Data;
using MineLib.Network.IO;

namespace MineLib.Network.Packets.Server
{
    public struct SpawnExperienceOrbPacket : IPacket
    {
        public int EntityID;
        public Vector3 Vector3;
        public short Count;

        public const byte PacketID = 0x11;
        public byte Id { get { return PacketID; } }

        public void ReadPacket(PacketByteReader reader)
        {
            EntityID = reader.ReadVarInt();
            Vector3 = Vector3.FromReaderIntFixedPoint(reader);
            Count = reader.ReadShort();
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(Id);
            stream.WriteVarInt(EntityID);
            Vector3.ToStreamIntFixedPoint(ref stream);
            stream.WriteShort(Count);
            stream.Purge();
        }
    }
}