using MineLib.Network.IO;
using MineLib.Network.Packets;

namespace MineLib.Network.Classic.Packets.Extension
{
    public struct HoldThisPacket : IPacket
    {
        public byte BlockToHold;
        public byte PreventChange;

        public const byte PacketID = 0x14;
        public byte ID { get { return PacketID; } }

        public void ReadPacket(PacketByteReader stream)
        {
            BlockToHold = stream.ReadByte();
            PreventChange = stream.ReadByte();
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteByte(ID);
            stream.WriteByte(BlockToHold);
            stream.WriteByte(PreventChange);
            stream.Purge();
        }
    }
}
