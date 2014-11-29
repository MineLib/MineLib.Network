using MineLib.Network.IO;

namespace MineLib.Network.PocketEdition.Packets.Client
{
    public class SetTimePacket : IPacketWithSize
    {
        public int Time;

        public byte ID { get { return 0x82; } }
        public short Size { get { return 0; } }

        public IPacketWithSize ReadPacket(IMinecraftDataReader reader)
        {
            Time = reader.ReadInt();

            return this;
        }

        IPacket IPacket.ReadPacket(IMinecraftDataReader reader)
        {
            return ReadPacket(reader);
        }

        public IPacket WritePacket(IMinecraftStream stream)
        {
            stream.WriteVarInt(ID);
            stream.WriteInt(Time);
            stream.Purge();

            return this;
        }
    }
}
