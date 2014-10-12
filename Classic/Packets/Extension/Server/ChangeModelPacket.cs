using MineLib.Network.IO;

namespace MineLib.Network.Classic.Packets.Extension.Server
{
    public struct ChangeModelPacket : IPacketWithSize
    {
        public byte EntityID;
        public string ModelName;

        public byte ID { get { return 0x1D; } }
        public short Size { get { return 66; } }

        public void ReadPacket(PacketByteReader stream)
        {
            EntityID = stream.ReadByte();
            ModelName = stream.ReadString();
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteByte(ID);
            stream.WriteByte(EntityID);
            stream.WriteString(ModelName);
            stream.Purge();
        }
    }
}
