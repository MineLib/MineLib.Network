using CWrapped;

namespace MineLib.Network.Packets.Server
{
    public struct MapsPacket : IPacket
    {
        public int ItemDamage;
        public byte[] Data;

        public const byte PacketID = 0x34;
        public byte Id { get { return PacketID; } }

        public void ReadPacket(ref Wrapped stream)
        {
            ItemDamage = stream.ReadVarInt();
            var length = stream.ReadShort();
            Data = stream.ReadByteArray(length);
        }

        public void WritePacket(ref Wrapped stream)
        {
            stream.WriteVarInt(Id);
            stream.WriteVarInt(ItemDamage);
            stream.WriteShort((short)Data.Length);
            stream.WriteByteArray(Data);
            stream.Purge();
        }
    }
}