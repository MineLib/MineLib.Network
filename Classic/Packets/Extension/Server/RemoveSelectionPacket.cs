using MineLib.Network.IO;

namespace MineLib.Network.Classic.Packets.Extension.Server
{
    public struct RemoveSelectionPacket : IPacketWithSize
    {
        public byte SelectionID;

        public byte ID { get { return 0x1B; } }
        public short Size { get { return 2; } }

        public IPacketWithSize ReadPacket(IMinecraftDataReader stream)
        {
            SelectionID = stream.ReadByte();

            return this;
        }

        IPacket IPacket.ReadPacket(IMinecraftDataReader stream)
        {
            return ReadPacket(stream);
        }

        public IPacket WritePacket(IMinecraftStream stream)
        {
            stream.WriteByte(ID);
            stream.WriteByte(SelectionID);
            stream.Purge();

            return this;
        }
    }
}
