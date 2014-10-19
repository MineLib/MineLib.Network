using MineLib.Network.IO;

namespace MineLib.Network.PocketEdition.Packets.Server
{
    public class LoginStatusPacket : IPacketWithSize
    {
        public int Status;

        public byte ID { get { return 0x83; } }
        public short Size { get { return 0; } }

        public IPacketWithSize ReadPacket(MinecraftDataReader reader)
        {
            Status = reader.ReadInt();

            return this;
        }

        IPacket IPacket.ReadPacket(MinecraftDataReader stream)
        {
            return ReadPacket(stream);
        }

        public IPacket WritePacket(MinecraftStream stream)
        {
            stream.WriteVarInt(ID);
            stream.WriteInt(Status);
            stream.Purge();

            return this;
        }
    }
}
