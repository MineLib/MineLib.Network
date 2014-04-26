using MineLib.Network.Data;
using MineLib.Network.IO;

namespace MineLib.Network.Packets.Server
{
    public struct SoundEffectPacket : IPacket
    {
        public string SoundName;
        public Coordinates3D Coordinates;
        public float Volume;
        public byte Pitch;

        public const byte PacketID = 0x29;
        public byte Id { get { return PacketID; } }

        public void ReadPacket(PacketByteReader stream)
        {
            SoundName = stream.ReadString();
            Coordinates.X = stream.ReadInt();
            Coordinates.Y = stream.ReadInt();
            Coordinates.Z = stream.ReadInt();
            Volume = stream.ReadFloat();
            Pitch = stream.ReadByte();
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(Id);
            stream.WriteString(SoundName);
            stream.WriteInt(Coordinates.X);
            stream.WriteInt(Coordinates.Y);
            stream.WriteInt(Coordinates.Z);
            stream.WriteFloat(Volume);
            stream.WriteByte(Pitch);
            stream.Purge();
        }
    }
}