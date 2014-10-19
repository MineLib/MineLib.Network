using MineLib.Network.IO;

namespace MineLib.Network.Modern.Packets.Server
{
    public struct HeldItemChangePacket : IPacket
    {
        public sbyte Slot;

        public byte ID { get { return 0x09; } }

        public IPacket ReadPacket(MinecraftDataReader reader)
        {
            Slot = reader.ReadSByte();

            return this;
        }

        public IPacket WritePacket(MinecraftStream stream)
        {
            stream.WriteVarInt(ID);
            stream.WriteSByte(Slot);
            stream.Purge();

            return this;
        }
    }
}