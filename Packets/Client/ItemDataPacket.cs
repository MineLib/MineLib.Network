using System.Text;
using CWrapped;

namespace MineLib.Network.Packets.Client
{
    public struct ItemDataPacket : IPacket
    {
        public short ItemType, ItemId;
        public string Text;

        public const byte PacketId = 0x83;
        public byte Id { get { return 0x83; } }

        public void ReadPacket(ref Wrapped stream)
        {
            ItemType = stream.ReadShort();
            ItemId = stream.ReadShort();
            var length = stream.ReadShort();
            Text = Encoding.ASCII.GetString(stream.ReadByteArray(length));
        }

        public void WritePacket(ref Wrapped stream)
        {
            stream.WriteVarInt(Id);
            stream.WriteShort(ItemType);
            stream.WriteShort(ItemId);
            stream.WriteShort((short)Text.Length);
            stream.WriteByteArray(Encoding.ASCII.GetBytes(Text));
            stream.Purge();
        }
    }
}