using MineLib.Network.IO;
using MineLib.Network.Packets;

namespace MineLib.Network.Classic.Packets.Server
{
    public struct DespawnPlayerPacket : IPacket
    {
        public sbyte PlayerID;

        public const byte PacketID = 0x0C;
        public byte ID { get { return PacketID; } }

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
