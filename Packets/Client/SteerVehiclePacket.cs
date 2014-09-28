using MineLib.Network.Enums;
using MineLib.Network.IO;

namespace MineLib.Network.Packets.Client
{
    public struct SteerVehiclePacket : IPacket
    {
        public float Sideways;
        public float Forward;
        public SteerVehicle Flags;

        public byte ID { get { return 0x0C; } }

        public void ReadPacket(PacketByteReader reader)
        {
            Sideways = reader.ReadFloat();
            Forward = reader.ReadFloat();
            Flags = (SteerVehicle) reader.ReadByte();
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(ID);
            stream.WriteFloat(Sideways);
            stream.WriteFloat(Forward);
            stream.WriteByte((byte) Flags);
            stream.Purge();
        }
    }
}