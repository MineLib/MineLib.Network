using MineLib.Network.IO;

namespace MineLib.Network.Main.Packets.Server
{
    public struct HeldItemChangePacket : IPacket
    {
        public sbyte Slot;

        public byte ID { get { return 0x09; } }

        public void ReadPacket(PacketByteReader reader)
        {
            Slot = reader.ReadSByte();
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(ID);
            stream.WriteSByte(Slot);
            stream.Purge();
        }
    }
}