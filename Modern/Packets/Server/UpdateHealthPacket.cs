using MineLib.Network.IO;

namespace MineLib.Network.Modern.Packets.Server
{
    public struct UpdateHealthPacket : IPacket
    {
        public float Health;
        public int Food;
        public float FoodSaturation;

        public byte ID { get { return 0x06; } }

        public IPacket ReadPacket(MinecraftDataReader reader)
        {
            Health = reader.ReadFloat();
            Food = reader.ReadVarInt();
            FoodSaturation = reader.ReadFloat();

            return this;
        }

        public IPacket WritePacket(MinecraftStream stream)
        {
            stream.WriteVarInt(ID);
            stream.WriteFloat(Health);
            stream.WriteVarInt(Food);
            stream.WriteFloat(FoodSaturation);
            stream.Purge();

            return this;
        }
    }
}