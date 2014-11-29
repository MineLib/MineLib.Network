using MineLib.Network.Data;
using MineLib.Network.IO;

namespace MineLib.Network.Modern.Packets.Server
{
    public struct SpawnGlobalEntityPacket : IPacket
    {
        public int EntityID;
        public sbyte Type;
        public Vector3 Vector3;

        public byte ID { get { return 0x2C; } }

        public IPacket ReadPacket(IMinecraftDataReader reader)
        {
            EntityID = reader.ReadVarInt();
            Type = reader.ReadSByte();
            Vector3 = Vector3.FromReaderIntFixedPoint(reader);

            return this;
        }

        public IPacket WritePacket(IMinecraftStream stream)
        {
            stream.WriteVarInt(ID);
            stream.WriteVarInt(EntityID);
            stream.WriteSByte(Type);
            Vector3.ToStreamIntFixedPoint(stream);
            stream.Purge();

            return this;
        }
    }
}