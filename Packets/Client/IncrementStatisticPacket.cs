using CWrapped;

namespace MineLib.Network.Packets.Client
{
    public struct IncrementStatisticPacket : IPacket
    {
        public int StatisticId;
        public int Amount;

        public const byte PacketId = 0xC8;
        public byte Id { get { return 0xC8; } }

        public void ReadPacket(ref Wrapped stream)
        {
            StatisticId = stream.ReadShort();
            Amount = stream.ReadShort();
        }

        public void WritePacket(ref Wrapped stream)
        {
            stream.WriteVarInt(Id);
            stream.WriteVarInt(StatisticId);
            stream.WriteVarInt(Amount);
            stream.Purge();
        }
    }
}