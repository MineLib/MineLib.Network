using CWrapped;

namespace MineLib.Network.Packets.Client
{
    public struct PlayerPacket : IPacket
    {
        public bool OnGround;

        public const byte PacketID = 0x03;
        public byte Id { get { return PacketID; } }

        public void ReadPacket(ref Wrapped stream)
        {
            OnGround = stream.ReadBool();
        }

        public void WritePacket(ref Wrapped stream)
        {
            stream.WriteVarInt(Id);
            stream.WriteBool(OnGround);
            stream.Purge();
        }
    }
}