using MineLib.Network.IO;

namespace MineLib.Network.Main.Packets.Server
{
    public struct DestroyEntitiesPacket : IPacket
    {
        public int[] EntityIDs;

        public byte ID { get { return 0x13; } }

        public void ReadPacket(PacketByteReader reader)
        {
            var count = reader.ReadVarInt();
            EntityIDs = reader.ReadVarIntArray(count);
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(ID);
            stream.WriteVarInt(EntityIDs.Length);
            stream.WriteVarIntArray(EntityIDs);
            stream.Purge();
        }
    }
}