using MineLib.Network.IO;
using MineLib.Network.Modern.Data;

namespace MineLib.Network.Modern.Packets.Server
{
    public struct EntityTeleportPacket : IPacket
    {
        public int EntityID;
        public Vector3 Vector3;
        public sbyte Yaw, Pitch;
        public bool OnGround;

        public byte ID { get { return 0x18; } }

        public IPacket ReadPacket(IMinecraftDataReader reader)
        {
            EntityID = reader.ReadVarInt();
            Vector3 = Vector3.FromReaderIntFixedPoint(reader);
            Yaw = reader.ReadSByte();
            Pitch = reader.ReadSByte();
            OnGround = reader.ReadBoolean();

            return this;
        }

        public IPacket WritePacket(IMinecraftStream stream)
        {
            stream.WriteVarInt(ID);
            stream.WriteVarInt(EntityID);
            Vector3.ToStreamIntFixedPoint(stream);
            stream.WriteSByte(Yaw);
            stream.WriteSByte(Pitch);
            stream.WriteBoolean(OnGround);
            stream.Purge();

            return this;
        }
    }
}