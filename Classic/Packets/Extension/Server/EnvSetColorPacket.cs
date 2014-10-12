using MineLib.Network.Classic.Enums;
using MineLib.Network.IO;

namespace MineLib.Network.Classic.Packets.Extension.Server
{
    public struct EnvSetColorPacket : IPacketWithSize
    {
        public EnvironmentalVariable Variable;
        public short Red;
        public short Green;
        public short Blue;

        public byte ID { get { return 0x19; } }
        public short Size { get { return 8; } }

        public void ReadPacket(PacketByteReader stream)
        {
            Variable = (EnvironmentalVariable) stream.ReadByte();
            Red = stream.ReadShort();
            Green = stream.ReadShort();
            Blue = stream.ReadShort();
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteByte(ID);
            stream.WriteByte((byte) Variable);
            stream.WriteShort(Red);
            stream.WriteShort(Green);
            stream.WriteShort(Blue);
            stream.Purge();
        }
    }
}
