using MineLib.Network.IO;
using MineLib.Network.Main.Enums;

namespace MineLib.Network.Main.Packets.Client
{
    public struct ClientStatusPacket : IPacket
    {
        public ClientStatus Status;

        public byte ID { get { return 0x16; } }

        public void ReadPacket(PacketByteReader reader)
        {
            Status = (ClientStatus) reader.ReadByte();
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(ID);
            stream.WriteByte((byte) Status);
            stream.Purge();
        }
    }
}