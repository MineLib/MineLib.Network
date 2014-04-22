using MineLib.Network.IO;

namespace MineLib.Network.Packets.Client
{
    public struct HeldItemChangPacket : IPacket
    {
        public short Slot;

        public const byte PacketID = 0x09;
        public byte Id { get { return PacketID; } }

        public void ReadPacket(PacketByteReader stream)
        {
            Slot = stream.ReadShort();
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(Id);
            stream.WriteShort(Slot);
            stream.Purge();
        }
    }
}