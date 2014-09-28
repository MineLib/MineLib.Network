using MineLib.Network.IO;

namespace MineLib.Network.Packets.Client
{
    public struct HeldItemChangPacket : IPacket
    {
        public short Slot;

        public byte ID { get { return 0x09; } }

        public void ReadPacket(PacketByteReader reader)
        {
            Slot = reader.ReadShort();
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(ID);
            stream.WriteShort(Slot);
            stream.Purge();
        }
    }
}