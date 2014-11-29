using MineLib.Network.IO;

namespace MineLib.Network.Classic.Packets.Server
{
    public struct MessagePacket : IPacketWithSize
    {
        public sbyte PlayerID;
        public string Message;

        public byte ID { get { return 0x0D; } }
        public short Size { get { return 66; } }

        public IPacketWithSize ReadPacket(IMinecraftDataReader reader)
        {
            PlayerID = reader.ReadSByte();
            Message = reader.ReadString();

            return this;
        }

        IPacket IPacket.ReadPacket(IMinecraftDataReader reader)
        {
            return ReadPacket(reader);
        }

        public IPacket WritePacket(IMinecraftStream stream)
        {
            stream.WriteByte(ID);
            stream.WriteSByte(PlayerID);
            stream.WriteString(Message);
            stream.Purge();

            return this;
        }
    }
}
