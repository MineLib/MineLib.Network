using MineLib.Network.IO;

namespace MineLib.Network.Modern.Packets.Server
{
    public struct AttachEntityPacket : IPacket
    {
        public int EntityID;
        public int VehicleID;
        public bool Leash;

        public byte ID { get { return 0x1B; } }

        public IPacket ReadPacket(MinecraftDataReader reader)
        {
            EntityID = reader.ReadInt();
            VehicleID = reader.ReadInt();
            Leash = reader.ReadBoolean();

            return this;
        }

        public IPacket WritePacket(MinecraftStream stream)
        {
            stream.WriteVarInt(ID);
            stream.WriteInt(EntityID);
            stream.WriteInt(VehicleID);
            stream.WriteBoolean(Leash);
            stream.Purge();

            return this;
        }
    }
}