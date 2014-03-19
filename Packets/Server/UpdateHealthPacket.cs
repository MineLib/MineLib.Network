using MineLib.Network.IO;


namespace MineLib.Network.Packets.Server
{
    public struct UpdateHealthPacket : IPacket
    {
        public float Health;
        public short Food;
        public float FoodSaturation;

        public const byte PacketID = 0x06;
        public byte Id { get { return PacketID; } }

        public void ReadPacket(PacketByteReader stream)
        {
            Health = stream.ReadFloat();
            Food = stream.ReadShort();
            FoodSaturation = stream.ReadFloat();
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(Id);
            stream.WriteFloat(Health);
            stream.WriteShort(Food);
            stream.WriteFloat(FoodSaturation);
            stream.Purge();
        }
    }
}