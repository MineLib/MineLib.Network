using MineLib.Network.IO;

namespace MineLib.Network.Modern.Packets.Server
{
    public struct CollectItemPacket : IPacket
    {
        public int CollectedEntityID;
        public int CollectorEntityID;

        public byte ID { get { return 0x0D; } }

        public IPacket ReadPacket(IMinecraftDataReader reader)
        {
            CollectedEntityID = reader.ReadVarInt();
            CollectorEntityID = reader.ReadVarInt();

            return this;
        }

        public IPacket WritePacket(IMinecraftStream stream)
        {
            stream.WriteVarInt(ID);
            stream.WriteVarInt(CollectedEntityID);
            stream.WriteVarInt(CollectorEntityID);
            stream.Purge();

            return this;
        }
    }
}