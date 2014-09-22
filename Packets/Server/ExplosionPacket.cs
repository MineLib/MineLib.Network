using MineLib.Network.IO;

namespace MineLib.Network.Packets.Server
{
    public struct ExplosionPacket : IPacket
    {
        public float X, Y, Z;
        public float Radius;
        public int RecordCount;
        public byte[] Records; // TODO: Records in ExplosionPacket
        public float PlayerMotionX, PlayerMotionY, PlayerMotionZ;

        public const byte PacketID = 0x27;
        public byte Id { get { return PacketID; } }

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
            stream.WriteVarInt(Id);
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