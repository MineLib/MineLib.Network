using MineLib.Network.IO;

namespace MineLib.Network.Packets.Server
{
    public struct AttachEntityPacket : IPacket
    {
        public int EntityID;
        public int VehicleID;
        public bool Leash;

        public byte ID { get { return 0x1B; } }

        public void ReadPacket(PacketByteReader reader)
        {
            EntityID = reader.ReadInt();
            VehicleID = reader.ReadInt();
            Leash = reader.ReadBoolean();
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(ID);
            stream.WriteInt(EntityID);
            stream.WriteInt(VehicleID);
            stream.WriteBoolean(Leash);
            stream.Purge();
        }
    }
}