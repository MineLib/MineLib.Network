using MineLib.Network.IO;

namespace MineLib.Network.Packets.Server
{
    public struct UpdateScorePacket : IPacket
    {
        public string ItemName;
        public bool RemoveItem; // Will be converted to byte 0-1
        public string ScoreName;
        public int? Value;

        public const byte PacketID = 0x3C;
        public byte Id { get { return PacketID; } }

        public void ReadPacket(PacketByteReader reader)
        {
            ItemName = reader.ReadString();
            RemoveItem = reader.ReadBoolean();
            if (!RemoveItem)
            {
                ScoreName = reader.ReadString();
                Value = reader.ReadInt();
            }
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(Id);
            stream.WriteString(ItemName);
            stream.WriteBoolean(RemoveItem);
            if (!RemoveItem)
            {
                stream.WriteString(ScoreName);
                stream.WriteInt(Value.Value);
            }
            stream.Purge();
        }
    }
}