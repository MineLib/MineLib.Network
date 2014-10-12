using MineLib.Network.IO;

namespace MineLib.Network.PocketEdition.Packets.Server
{
    public class LoginStatusPacket : IPacketWithSize
    {
        public int Status;

        public byte ID { get { return 0x83; } }
        public short Size { get { return 0; } }

        public void ReadPacket(PacketByteReader reader)
        {
            Status = reader.ReadInt();
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(ID);
            stream.WriteInt(Status);
            stream.Purge();
        }
    }
}
