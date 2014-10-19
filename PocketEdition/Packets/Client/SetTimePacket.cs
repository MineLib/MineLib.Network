using MineLib.Network.IO;

namespace MineLib.Network.PocketEdition.Packets.Client
{
    public class SetTimePacket : IPacketWithSize
    {
        public int Time;

        public byte ID { get { return 0x82; } }
        public short Size { get { return 0; } }

        public IPacketWithSize ReadPacket(MinecraftDataReader reader)
        {
            Time = reader.ReadInt();

            return this;
        }

        IPacket IPacket.ReadPacket(MinecraftDataReader stream)
        {
            return ReadPacket(stream);
        }

        public IPacket WritePacket(MinecraftStream stream)
        {
            stream.WriteVarInt(ID);
            stream.WriteInt(Time);
            stream.Purge();

            return this;
        }
    }
}
