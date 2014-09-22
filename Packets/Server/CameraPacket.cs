using MineLib.Network.IO;

namespace MineLib.Network.Packets.Server
{
    public struct CameraPacket : IPacket
    {
        public int CameraID;

        public const byte PacketID = 0x43;
        public byte Id { get { return PacketID; } }

        public void ReadPacket(PacketByteReader reader)
        {
            CameraID = reader.ReadVarInt();
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(Id);
            stream.WriteVarInt(CameraID);
            stream.Purge();
        }
    }
}
