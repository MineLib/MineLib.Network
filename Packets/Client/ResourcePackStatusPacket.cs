using MineLib.Network.Enums;
using MineLib.Network.IO;

namespace MineLib.Network.Packets.Client
{
    public struct ResourcePackStatusPacket : IPacket
    {
        public string Hash;
        public ResourcePackStatus Result;

        public const byte PacketID = 0x19;
        public byte Id { get { return PacketID; } }

        public void ReadPacket(PacketByteReader reader)
        {
            Hash = reader.ReadString();
            Result = (ResourcePackStatus) reader.ReadVarInt();
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(Id);
            stream.WriteString(Hash);
            stream.WriteVarInt((int) Result);
            stream.Purge();
        }
    }
}
