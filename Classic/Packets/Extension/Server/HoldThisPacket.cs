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

        public IPacketWithSize ReadPacket(IMinecraftDataReader reader)
        {
            BlockToHold = reader.ReadByte();
            PreventChange = (PreventChange) reader.ReadByte();

            return this;
        }

        IPacket IPacket.ReadPacket(IMinecraftDataReader reader)
        {
            return ReadPacket(reader);
        }

        public IPacket WritePacket(IMinecraftStream stream)
        {
            stream.WriteByte(ID);
            stream.WriteByte(BlockToHold);
            stream.WriteByte((byte) PreventChange);
            stream.Purge();

            return this;
        }
    }
}
