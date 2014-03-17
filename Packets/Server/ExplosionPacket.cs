using MineLib.Network.IO;


namespace MineLib.Network.Packets.Server
{
    public struct ExplosionPacket : IPacket
    {
        public float X, Y, Z;
        public float Radius;
        public int RecordCount;
        public byte[] Records;
        public float PlayerMotionX, PlayerMotionY, PlayerMotionZ;

        public const byte PacketID = 0x27;
        public byte Id { get { return PacketID; } }

        public void ReadPacket(ref Wrapped stream)
        {
            X = stream.ReadFloat();
            Y = stream.ReadFloat();
            Z = stream.ReadFloat();
            Radius = stream.ReadFloat();
            RecordCount = stream.ReadInt();
            Records = stream.ReadByteArray(RecordCount * 3);
            PlayerMotionX = stream.ReadFloat();
            PlayerMotionY = stream.ReadFloat();
            PlayerMotionZ = stream.ReadFloat();
        }

        public void WritePacket(ref Wrapped stream)
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