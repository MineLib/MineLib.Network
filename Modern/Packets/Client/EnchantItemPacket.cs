using MineLib.Network.IO;

namespace MineLib.Network.Modern.Packets.Client
{
    public struct EnchantItemPacket : IPacket
    {
        public byte WindowId;
        public byte Enchantment;

        public byte ID { get { return 0x11; } }

        public IPacket ReadPacket(MinecraftDataReader reader)
        {
            WindowId = reader.ReadByte();
            Enchantment = reader.ReadByte();

            return this;
        }

        public IPacket WritePacket(MinecraftStream stream)
        {
            stream.WriteVarInt(ID);
            stream.WriteVarInt(WindowId);
            stream.WriteVarInt(Enchantment);
            stream.Purge();

            return this;
        }
    }
}