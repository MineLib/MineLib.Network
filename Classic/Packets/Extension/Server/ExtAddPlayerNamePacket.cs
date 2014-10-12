using MineLib.Network.IO;

namespace MineLib.Network.Classic.Packets.Extension.Server
{
    public struct ExtAddPlayerNamePacket : IPacketWithSize
    {
        public short NameID;
        public string PlayerName;
        public string ListName;
        public string GroupName;
        public byte GroupRank;

        public byte ID { get { return 0x16; } }
        public short Size { get { return 196; } }

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
            stream.WriteByte(ID);
            stream.WriteShort(NameID);
            stream.WriteString(PlayerName);
            stream.WriteString(ListName);
            stream.WriteString(GroupName);
            stream.WriteByte(GroupRank);
            stream.Purge();
        }
    }
}
