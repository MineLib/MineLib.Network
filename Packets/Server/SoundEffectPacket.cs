using MineLib.Network.Data;
using MineLib.Network.IO;

namespace MineLib.Network.Packets.Server
{
    public struct SoundEffectPacket : IPacket
    {
        public string SoundName;
        public Position Coordinates;
        public float Volume;
        public byte Pitch;

        public const byte PacketID = 0x29;
        public byte Id { get { return PacketID; } }

        public void ReadPacket(PacketByteReader reader)
        {
            SoundName = reader.ReadString();
            Coordinates = Position.FromReaderInt(reader);
            Volume = reader.ReadFloat();
            Pitch = reader.ReadByte();
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(Id);
            stream.WriteString(SoundName);
            Coordinates.ToStreamInt(ref stream);
            stream.WriteFloat(Volume);
            stream.WriteByte(Pitch);
            stream.Purge();
        }
    }
}