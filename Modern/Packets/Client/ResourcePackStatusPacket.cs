using MineLib.Network.IO;
using MineLib.Network.Modern.Enums;

namespace MineLib.Network.Modern.Packets.Client
{
    public struct ResourcePackStatusPacket : IPacket
    {
        public string Hash;
        public ResourcePackStatus Result;

        public byte ID { get { return 0x19; } }

        public IPacket ReadPacket(MinecraftDataReader reader)
        {
            Hash = reader.ReadString();
            Result = (ResourcePackStatus) reader.ReadVarInt();

            return this;
        }

        public IPacket WritePacket(MinecraftStream stream)
        {
            stream.WriteVarInt(ID);
            stream.WriteString(Hash);
            stream.WriteVarInt((int) Result);
            stream.Purge();

            return this;
        }
    }
}
