using MineLib.Network.Classic.Enums;
using MineLib.Network.IO;

namespace MineLib.Network.Classic.Packets.Extension.Server
{
    public struct HoldThisPacket : IPacketWithSize
    {
        public byte BlockToHold;
        public PreventChange PreventChange;

        public byte ID { get { return 0x14; } }
        public short Size { get { return 3; } }

        public IPacketWithSize ReadPacket(MinecraftDataReader stream)
        {
            BlockToHold = stream.ReadByte();
            PreventChange = (PreventChange) stream.ReadByte();

            return this;
        }

        IPacket IPacket.ReadPacket(MinecraftDataReader stream)
        {
            return ReadPacket(stream);
        }

        public IPacket WritePacket(MinecraftStream stream)
        {
            stream.WriteByte(ID);
            stream.WriteByte(BlockToHold);
            stream.WriteByte((byte) PreventChange);
            stream.Purge();

            return this;
        }
    }
}
