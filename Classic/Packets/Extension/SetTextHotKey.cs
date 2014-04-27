using MineLib.Network.IO;
using MineLib.Network.Packets;

namespace MineLib.Network.Classic.Packets.Extension
{
    public struct SetTextHotKeyPacket : IPacket
    {
        public string Label;
        public string Action;
        public int KeyCode;
        public byte KeyMods;

        public const byte PacketID = 0x15;
        public byte Id { get { return PacketID; } }

        public void ReadPacket(PacketByteReader stream)
        {
            Label = stream.ReadString();
            Action = stream.ReadString();
            KeyCode = stream.ReadInt();
            KeyMods = stream.ReadByte();
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteByte(Id);
            stream.WriteString(Label);
            stream.WriteString(Action);
            stream.WriteInt(KeyCode);
            stream.WriteByte(KeyMods);
            stream.Purge();
        }
    }
}
