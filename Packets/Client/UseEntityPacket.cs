using CWrapped;

namespace MineLib.Network.Packets.Client
{
    public struct UseEntityPacket : IPacket
    {
        public int User, Target;
        public bool LeftClick;

        public const byte PacketId = 0x07;
        public byte Id { get { return 0x07; } }

        public void ReadPacket(ref Wrapped stream)
        {
            User = stream.ReadShort();
            Target = stream.ReadShort();
            LeftClick = stream.ReadBool();
        }

        public void WritePacket(ref Wrapped stream)
        {
            stream.WriteVarInt(Id);
            stream.WriteVarInt(User);
            stream.WriteVarInt(Target);
            stream.WriteBool(LeftClick);
            stream.Purge();
        }
    }
}