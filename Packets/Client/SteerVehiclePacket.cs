using CWrapped;

namespace MineLib.Network.Packets.Client
{
    public struct SteerVehiclePacket : IPacket
    {
        public float Strafe;
        public float Forward;
        public bool Jump;
        public bool Unmount;

        public const byte PacketId = 0x1B;
        public byte Id { get { return 0x1B; } }

        public void ReadPacket(ref Wrapped stream)
        {
            Strafe = stream.ReadFloat();
            Forward = stream.ReadFloat();
            Jump = stream.ReadBool();
            Unmount = stream.ReadBool();
        }

        public void WritePacket(ref Wrapped stream)
        {
            stream.WriteVarInt(Id);
            stream.WriteFloat(Strafe);
            stream.WriteFloat(Forward);
            stream.WriteBool(Jump);
            stream.WriteBool(Unmount);
            stream.Purge();
        }
    }
}