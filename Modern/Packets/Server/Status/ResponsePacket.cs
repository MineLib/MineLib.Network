using MineLib.Network.IO;

namespace MineLib.Network.Modern.Packets.Server.Status
{
    public struct ResponsePacket : IPacket
    {
        public string Response;

        public byte ID { get { return 0x00; } }

        public IPacket ReadPacket(MinecraftDataReader reader)
        {
            Response = reader.ReadString();

            return this;
        }

        public IPacket WritePacket(MinecraftStream stream)
        {
            stream.WriteVarInt(ID);
            stream.WriteString(Response);
            stream.Purge();

            return this;
        }
    }
}