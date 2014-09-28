using MineLib.Network.IO;

namespace MineLib.Network.Packets.Client
{
    public struct PlayerPacket : IPacket
    {
        public bool OnGround;

        public byte ID { get { return 0x03; } }

        public void ReadPacket(PacketByteReader reader)
        {
            OnGround = reader.ReadBoolean();
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(ID);
            stream.WriteBoolean(OnGround);
            stream.Purge();
        }
    }
}