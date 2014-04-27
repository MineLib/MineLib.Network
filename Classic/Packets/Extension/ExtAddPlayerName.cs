using MineLib.Network.IO;
using MineLib.Network.Packets;

namespace MineLib.Network.Classic.Packets.Extension
{
    public struct ExtAddPlayerNamePacket : IPacket
    {
        public short NameID;
        public string PlayerName;
        public string ListName;
        public string GroupName;
        public byte GroupRank;

        public const byte PacketID = 0x16;
        public byte Id { get { return PacketID; } }

        public void ReadPacket(PacketByteReader stream)
        {
            NameID = stream.ReadShort();
            PlayerName = stream.ReadString();
            ListName = stream.ReadString();
            GroupName = stream.ReadString();
            GroupRank = stream.ReadByte();
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteByte(Id);
            stream.WriteShort(NameID);
            stream.WriteString(PlayerName);
            stream.WriteString(ListName);
            stream.WriteString(GroupName);
            stream.WriteByte(GroupRank);
            stream.Purge();
        }
    }
}
