using CWrapped;

namespace MineLib.Network.Packets.Server
{
    public struct AttachEntityPacket : IPacket
    {
        public int EntityID, VehicleID;
        public bool Leash;

        public const byte PacketId = 0x1B;
        public byte Id { get { return 0x1B; } }

        public void ReadPacket(ref Wrapped stream)
        {
            EntityID = stream.ReadInt();
            VehicleID = stream.ReadInt();
            Leash = stream.ReadBool();
        }

        public void WritePacket(ref Wrapped stream)
        {
            stream.WriteVarInt(Id);
            stream.WriteInt(EntityID);
            stream.WriteInt(VehicleID);
            stream.WriteBool(Leash);
            stream.Purge();
        }
    }
}