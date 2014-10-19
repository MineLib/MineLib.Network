using MineLib.Network.IO;

namespace MineLib.Network.Modern.Packets.Client
{
    public struct HeldItemChangPacket : IPacket
    {
        public short Slot;

        public byte ID { get { return 0x09; } }

        public IPacket ReadPacket(MinecraftDataReader reader)
        {
            Slot = reader.ReadShort();

            return this;
        }

        public IPacket WritePacket(MinecraftStream stream)
        {
            stream.WriteVarInt(ID);
            stream.WriteShort(Slot);
            stream.Purge();

            return this;
        }
    }
}