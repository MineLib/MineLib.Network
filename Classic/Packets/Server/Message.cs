using MineLib.Network.IO;

namespace MineLib.Network.Classic.Packets.Server
{
    public struct MessagePacket : IPacketWithSize
    {
        public sbyte PlayerID;
        public string Message;

        public byte ID { get { return 0x0D; } }
        public short Size { get { return 66; } }

        public IPacketWithSize ReadPacket(MinecraftDataReader stream)
        {
            PlayerID = stream.ReadSByte();
            Message = stream.ReadString();

            return this;
        }

        IPacket IPacket.ReadPacket(MinecraftDataReader stream)
        {
            return ReadPacket(stream);
        }

        public IPacket WritePacket(MinecraftStream stream)
        {
            stream.WriteByte(ID);
            stream.WriteSByte(PlayerID);
            stream.WriteString(Message);
            stream.Purge();

            return this;
        }
    }
}
