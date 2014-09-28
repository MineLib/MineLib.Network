using MineLib.Network.IO;

namespace MineLib.Network.Packets.Server
{
    public struct CameraPacket : IPacket
    {
        public int CameraID;

        public byte ID { get { return 0x43; } }

        public void ReadPacket(PacketByteReader reader)
        {
            CameraID = reader.ReadVarInt();
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(ID);
            stream.WriteVarInt(CameraID);
            stream.Purge();
        }
    }
}
