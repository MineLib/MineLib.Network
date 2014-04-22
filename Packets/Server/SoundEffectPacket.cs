using MineLib.Network.Data;
using MineLib.Network.IO;

namespace MineLib.Network.Packets.Server
{
    public struct SoundEffectPacket : IPacket
    {
        public string SoundName;
        public Vector3 Vector3;
        public float Volume;
        public byte Pitch;

        public const byte PacketID = 0x29;
        public byte Id { get { return PacketID; } }

        public void ReadPacket(PacketByteReader stream)
        {
            SoundName = stream.ReadString();
            Vector3.X = stream.ReadInt();
            Vector3.Y = stream.ReadInt();
            Vector3.Z = stream.ReadInt();
            Volume = stream.ReadFloat();
            Pitch = stream.ReadByte();
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(Id);
            stream.WriteString(SoundName);
            stream.WriteInt((int)Vector3.X);
            stream.WriteInt((int)Vector3.Y);
            stream.WriteInt((int)Vector3.Z);
            stream.WriteFloat(Volume);
            stream.WriteByte(Pitch);
            stream.Purge();
        }
    }
}