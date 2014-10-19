using MineLib.Network.IO;

namespace MineLib.Network.Classic.Packets.Server
{
    public struct DespawnPlayerPacket : IPacketWithSize
    {
        public sbyte PlayerID;

        public byte ID { get { return 0x0C; } }
        public short Size { get { return 2; } }

        public IPacketWithSize ReadPacket(MinecraftDataReader stream)
        {
            PlayerID = stream.ReadSByte();

            return this;
        }

        IPacket IPacket.ReadPacket(MinecraftDataReader stream)
        {
            return ReadPacket(stream);
        }

        public IPacket WritePacket(MinecraftStream stream)
        {
            stream.WriteByte(ID);
            stream.WriteSByte(PlayerID);
            stream.Purge();

            return this;
        }
    }
}
