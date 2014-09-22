using MineLib.Network.IO;

namespace MineLib.Network.Packets.Server
{
    public struct UpdateHealthPacket : IPacket
    {
        public float Health;
        public int Food;
        public float FoodSaturation;

        public const byte PacketID = 0x06;
        public byte Id { get { return PacketID; } }

        public void ReadPacket(PacketByteReader reader)
        {
            Health = reader.ReadFloat();
            Food = reader.ReadVarInt();
            FoodSaturation = reader.ReadFloat();
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(Id);
            stream.WriteFloat(Health);
            stream.WriteVarInt(Food);
            stream.WriteFloat(FoodSaturation);
            stream.Purge();
        }
    }
}