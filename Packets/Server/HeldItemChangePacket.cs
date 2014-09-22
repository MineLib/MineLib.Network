using MineLib.Network.IO;

namespace MineLib.Network.Packets.Server
{
    public struct HeldItemChangePacket : IPacket
    {
        public sbyte Slot;

        public const byte PacketID = 0x09;
        public byte Id { get { return PacketID; } }

        public void ReadPacket(PacketByteReader reader)
        {
            Slot = reader.ReadSByte();
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(Id);
            stream.WriteSByte(Slot);
            stream.Purge();
        }
    }
}