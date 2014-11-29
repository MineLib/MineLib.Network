using MineLib.Network.Classic.Enums;
using MineLib.Network.IO;

namespace MineLib.Network.Classic.Packets.Extension.Server
{
    public struct SetTextHotKeyPacket : IPacketWithSize
    {
        public string Label;
        public string Action;
        public int KeyCode;
        public KeyMods KeyMods;

        public byte ID { get { return 0x15; } }
        public short Size { get { return 134; } }

        public IPacketWithSize ReadPacket(IMinecraftDataReader reader)
        {
            Label = reader.ReadString();
            Action = reader.ReadString();
            KeyCode = reader.ReadInt();
            KeyMods = (KeyMods) reader.ReadByte();

            return this;
        }

        IPacket IPacket.ReadPacket(IMinecraftDataReader reader)
        {
            return ReadPacket(reader);
        }

        public IPacket WritePacket(IMinecraftStream stream)
        {
            stream.WriteByte(ID);
            stream.WriteString(Label);
            stream.WriteString(Action);
            stream.WriteInt(KeyCode);
            stream.WriteByte((byte) KeyMods);
            stream.Purge();

            return this;
        }
    }
}
