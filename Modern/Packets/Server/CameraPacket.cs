using MineLib.Network.IO;

namespace MineLib.Network.Modern.Packets.Server
{
    public struct CameraPacket : IPacket
    {
        public int CameraID;

        public byte ID { get { return 0x43; } }

        public IPacket ReadPacket(IMinecraftDataReader reader)
        {
            CameraID = reader.ReadVarInt();

            return this;
        }

        public IPacket WritePacket(IMinecraftStream stream)
        {
            stream.WriteVarInt(ID);
            stream.WriteVarInt(CameraID);
            stream.Purge();

            return this;
        }
    }
}
