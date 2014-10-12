using MineLib.Network.IO;

namespace MineLib.Network.Main.Packets.Client
{
    public struct EnchantItemPacket : IPacket
    {
        public byte WindowId;
        public byte Enchantment;

        public byte ID { get { return 0x11; } }

        public void ReadPacket(PacketByteReader reader)
        {
            WindowId = reader.ReadByte();
            Enchantment = reader.ReadByte();
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(ID);
            stream.WriteVarInt(WindowId);
            stream.WriteVarInt(Enchantment);
            stream.Purge();
        }
    }
}