using MineLib.Network.IO;

namespace MineLib.Network.PocketEdition.Packets.Client
{
    public class MessagePacket : IPacketWithSize
    {
        public string Message;

        public byte ID { get { return 0x85; } }
        public short Size { get { return 0; } }

        public IPacketWithSize ReadPacket(IMinecraftDataReader reader)
        {
            Message = reader.ReadString();

            return this;
        }

        IPacket IPacket.ReadPacket(IMinecraftDataReader stream)
        {
            return ReadPacket(stream);
        }

        public IPacket WritePacket(IMinecraftStream stream)
        {
            stream.WriteVarInt(ID);
            stream.WriteString(Message);
            stream.Purge();

            return this;
        }
    }
}
