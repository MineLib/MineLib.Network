using CWrapped;

namespace MineLib.Network.Packets.Server
{
    public struct MapsPacket : IPacket
    {
        public int ItemDamage;
        public byte[] Data;

        public const byte PacketId = 0x34;
        public byte Id { get { return 0x34; } }

        public void ReadPacket(ref Wrapped stream)
        {
            ItemDamage = stream.ReadInt();
            var length = stream.ReadShort();
            Data = stream.ReadByteArray(length);
        }

        public void WritePacket(ref Wrapped stream)
        {
            stream.WriteVarInt(Id);
            stream.WriteInt(ItemDamage);
            stream.WriteShort((short)Data.Length);
            stream.WriteByteArray(Data);
            stream.Purge();
        }
    }
}