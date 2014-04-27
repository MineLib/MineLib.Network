using MineLib.Network.IO;

namespace MineLib.Network.Packets.Server
{
    public struct PlayerListItemPacket : IPacket
    {
        public string PlayerName;
        public bool Online;
        public short Ping;

        public const byte PacketID = 0x38;
        public byte Id { get { return PacketID; } }

        public void ReadPacket(PacketByteReader stream)
        {
            PlayerName = stream.ReadString();
            Online = stream.ReadBoolean();
            Ping = stream.ReadShort();
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(Id);
            stream.WriteString(PlayerName);
            stream.WriteBool(Online);
            stream.WriteShort(Ping);
            stream.Purge();
        }
    }
}