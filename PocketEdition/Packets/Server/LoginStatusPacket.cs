using MineLib.Network.IO;

namespace MineLib.Network.PocketEdition.Packets.Server
{
    public class LoginStatusPacket : IPacketWithSize
    {
        public int Status;

        public byte ID { get { return 0x83; } }
        public short Size { get { return 0; } }

        public IPacketWithSize ReadPacket(IMinecraftDataReader reader)
        {
            Status = reader.ReadInt();

            return this;
        }

        IPacket IPacket.ReadPacket(IMinecraftDataReader stream)
        {
            return ReadPacket(stream);
        }

        public IPacket WritePacket(IMinecraftStream stream)
        {
            stream.WriteVarInt(ID);
            stream.WriteInt(Status);
            stream.Purge();

            return this;
        }
    }
}
