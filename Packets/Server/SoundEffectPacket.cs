using MineLib.Network.IO;


namespace MineLib.Network.Packets.Server
{
    public struct SoundEffectPacket : IPacket
    {
        public string SoundName;
        public int X, Y, Z;
        public float Volume;
        public byte Pitch;

        public const byte PacketID = 0x29;
        public byte Id { get { return PacketID; } }

        public void ReadPacket(PacketByteReader stream)
        {
            SoundName = stream.ReadString();
            X = stream.ReadInt();
            Y = stream.ReadInt();
            Z = stream.ReadInt();
            Volume = stream.ReadFloat();
            Pitch = stream.ReadByte();
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(Id);
            stream.WriteString(SoundName);
            stream.WriteInt(X);
            stream.WriteInt(Y);
            stream.WriteInt(Z);
            stream.WriteFloat(Volume);
            stream.WriteByte(Pitch);
            stream.Purge();
        }
    }
}