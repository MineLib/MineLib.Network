using MineLib.Network.IO;

namespace MineLib.Network.Classic.Packets.Extension.Server
{
    public struct ExtRemovePlayerNamePacket : IPacketWithSize
    {
        public short NameID;

        public byte ID { get { return 0x18; } }
        public short Size { get { return 3; } }

        public void ReadPacket(PacketByteReader stream)
        {
            NameID = stream.ReadShort();
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteByte(ID);
            stream.WriteShort(NameID);
            stream.Purge();
        }
    }
}
