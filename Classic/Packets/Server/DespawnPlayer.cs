using MineLib.Network.IO;

namespace MineLib.Network.Classic.Packets.Server
{
    public struct DespawnPlayerPacket : IPacketWithSize
    {
        public sbyte PlayerID;

        public byte ID { get { return 0x0C; } }
        public short Size { get { return 2; } }

        public void ReadPacket(PacketByteReader stream)
        {
            PlayerID = stream.ReadSByte();
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteByte(ID);
            stream.WriteSByte(PlayerID);
            stream.Purge();
        }
    }
}
