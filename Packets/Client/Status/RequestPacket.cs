using MineLib.Network.IO;


namespace MineLib.Network.Packets.Client.Status
{
    public struct RequestPacket : IPacket
    {
        public const byte PacketID = 0x00;
        public byte Id { get { return PacketID; } }

        public void ReadPacket(PacketByteReader stream)
        {
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(Id);
            stream.Purge();
        }

    }
}
