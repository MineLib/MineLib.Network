using CWrapped;

namespace MineLib.Network.Packets.Server
{
    public struct UpdateScorePacket : IPacket
    {
        public string ItemName;
        public bool RemoveItem; // Will be converted to byte 0-1
        public string ScoreName;
        public int? Value;

        public const byte PacketId = 0x3C;
        public byte Id { get { return 0x3C; } }

        public void ReadPacket(ref Wrapped stream)
        {
            ItemName = stream.ReadString();
            RemoveItem = stream.ReadBool();
            if (!RemoveItem)
            {
                ScoreName = stream.ReadString();
                Value = stream.ReadInt();
            }
        }

        public void WritePacket(ref Wrapped stream)
        {
            stream.WriteVarInt(Id);
            stream.WriteString(ItemName);
            stream.WriteBool(RemoveItem);
            if (!RemoveItem)
            {
                stream.WriteString(ScoreName);
                stream.WriteInt(Value.Value);
            }
            stream.Purge();
        }
    }
}