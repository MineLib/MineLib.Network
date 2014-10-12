using MineLib.Network.IO;

namespace MineLib.Network.PocketEdition.Packets.Client
{
    public class SetTimePacket : IPacketWithSize
    {
        public int Time;

        public byte ID { get { return 0x82; } }
        public short Size { get { return 0; } }

        public void ReadPacket(PacketByteReader reader)
        {
            Time = reader.ReadInt();
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(ID);
            stream.WriteInt(Time);
            stream.Purge();
        }
    }
}
