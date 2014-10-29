using MineLib.Network.IO;

namespace MineLib.Network.Modern.Packets.Server
{
    public struct DestroyEntitiesPacket : IPacket
    {
        public int[] EntityIDs;

        public byte ID { get { return 0x13; } }

        public IPacket ReadPacket(IMinecraftDataReader reader)
        {
            var count = reader.ReadVarInt();
            EntityIDs = reader.ReadVarIntArray(count);

            return this;
        }

        public IPacket WritePacket(IMinecraftStream stream)
        {
            stream.WriteVarInt(ID);
            stream.WriteVarInt(EntityIDs.Length);
            stream.WriteVarIntArray(EntityIDs);
            stream.Purge();

            return this;
        }
    }
}