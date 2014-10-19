using MineLib.Network.IO;
using MineLib.Network.Modern.Data;

namespace MineLib.Network.Modern.Packets.Server
{
    public struct SpawnExperienceOrbPacket : IPacket
    {
        public int EntityID;
        public Vector3 Vector3;
        public short Count;

        public byte ID { get { return 0x11; } }

        public IPacket ReadPacket(MinecraftDataReader reader)
        {
            EntityID = reader.ReadVarInt();
            Vector3 = Vector3.FromReaderIntFixedPoint(reader);
            Count = reader.ReadShort();

            return this;
        }

        public IPacket WritePacket(MinecraftStream stream)
        {
            stream.WriteVarInt(ID);
            stream.WriteVarInt(EntityID);
            Vector3.ToStreamIntFixedPoint(stream);
            stream.WriteShort(Count);
            stream.Purge();

            return this;
        }
    }
}