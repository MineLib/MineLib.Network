using MineLib.Network.IO;

namespace MineLib.Network.Main.Packets.Server
{
    public struct ExplosionPacket : IPacket
    {
        public float X, Y, Z;
        public float Radius;
        public int RecordCount;
        public byte[] Records; // TODO: Records in ExplosionPacket
        public float PlayerMotionX, PlayerMotionY, PlayerMotionZ;

        public byte ID { get { return 0x27; } }

        public void ReadPacket(PacketByteReader reader)
        {
            X = reader.ReadFloat();
            Y = reader.ReadFloat();
            Z = reader.ReadFloat();
            Radius = reader.ReadFloat();
            RecordCount = reader.ReadInt();
            Records = reader.ReadByteArray(3 * RecordCount);
            PlayerMotionX = reader.ReadFloat();
            PlayerMotionY = reader.ReadFloat();
            PlayerMotionZ = reader.ReadFloat();
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(ID);
            stream.WriteFloat(X);
            stream.WriteFloat(Y);
            stream.WriteFloat(Z);
            stream.WriteFloat(Radius);
            stream.WriteInt(RecordCount);
            stream.WriteByteArray(Records);
            stream.WriteFloat(PlayerMotionX);
            stream.WriteFloat(PlayerMotionY);
            stream.WriteFloat(PlayerMotionZ);
            stream.Purge();
        }
    }
}