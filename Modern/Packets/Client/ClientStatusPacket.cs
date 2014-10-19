using MineLib.Network.IO;
using MineLib.Network.Modern.Enums;

namespace MineLib.Network.Modern.Packets.Client
{
    public struct ClientStatusPacket : IPacket
    {
        public ClientStatus Status;

        public byte ID { get { return 0x16; } }

        public IPacket ReadPacket(MinecraftDataReader reader)
        {
            Status = (ClientStatus) reader.ReadByte();

            return this;
        }

        public IPacket WritePacket(MinecraftStream stream)
        {
            stream.WriteVarInt(ID);
            stream.WriteByte((byte) Status);
            stream.Purge();

            return this;
        }
    }
}