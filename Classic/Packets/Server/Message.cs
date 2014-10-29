using MineLib.Network.IO;

namespace MineLib.Network.Classic.Packets.Server
{
    public struct MessagePacket : IPacketWithSize
    {
        public sbyte PlayerID;
        public string Message;

        public byte ID { get { return 0x0D; } }
        public short Size { get { return 66; } }

        public IPacketWithSize ReadPacket(IMinecraftDataReader stream)
        {
            PlayerID = stream.ReadSByte();
            Message = stream.ReadString();

            return this;
        }

        IPacket IPacket.ReadPacket(IMinecraftDataReader stream)
        {
            return ReadPacket(stream);
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
