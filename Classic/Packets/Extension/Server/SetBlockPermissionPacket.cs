using MineLib.Network.Classic.Enums;
using MineLib.Network.IO;

namespace MineLib.Network.Classic.Packets.Extension.Server
{
    public struct SetBlockPermissionPacket : IPacketWithSize
    {
        public byte BlockType;
        public AllowPlacement AllowPlacement;
        public AllowDeletion AllowDeletion;

        public byte ID { get { return 0x1C; } }
        public short Size { get { return 4; } }

        public IPacketWithSize ReadPacket(IMinecraftDataReader reader)
        {
            BlockType = reader.ReadByte();
            AllowPlacement = (AllowPlacement) reader.ReadByte();
            AllowDeletion = (AllowDeletion) reader.ReadByte();;

            return this;
        }

        IPacket IPacket.ReadPacket(IMinecraftDataReader reader)
        {
            return ReadPacket(reader);
        }

        public IPacket WritePacket(IMinecraftStream stream)
        {
            stream.WriteByte(ID);
            stream.WriteByte(BlockType);
            stream.WriteByte((byte) AllowPlacement);
            stream.WriteByte((byte) AllowDeletion);
            stream.Purge();

            return this;
        }
    }
}
