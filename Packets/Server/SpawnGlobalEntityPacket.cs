using MineLib.Network.Data;
using MineLib.Network.IO;

namespace MineLib.Network.Packets.Server
{
    public struct SpawnGlobalEntityPacket : IPacket
    {
        public int EntityID;
        public sbyte Type;
        public Vector3 Vector3;

        public byte ID { get { return 0x2C; } }

        public void ReadPacket(PacketByteReader reader)
        {
            EntityID = reader.ReadVarInt();
            Type = reader.ReadSByte();
            Vector3 = Vector3.FromReaderIntFixedPoint(reader);
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(ID);
            stream.WriteVarInt(EntityID);
            stream.WriteSByte(Type);
            Vector3.ToStreamIntFixedPoint(ref stream);
            stream.Purge();
        }
    }
}