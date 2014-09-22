using MineLib.Network.IO;

namespace MineLib.Network.Packets.Client
{
    public struct PlayerPacket : IPacket
    {
        public bool OnGround;

        public const byte PacketID = 0x03;
        public byte Id { get { return PacketID; } }

        public void ReadPacket(PacketByteReader reader)
        {
            OnGround = reader.ReadBoolean();
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(Id);
            stream.WriteBoolean(OnGround);
            stream.Purge();
        }
    }
}