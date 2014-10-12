using MineLib.Network.IO;
using MineLib.Network.Main.Data;

namespace MineLib.Network.Main.Packets.Server
{
    public struct SoundEffectPacket : IPacket
    {
        public string SoundName;
        public Position Coordinates;
        public float Volume;
        public byte Pitch;

        public byte ID { get { return 0x29; } }

        public void ReadPacket(PacketByteReader reader)
        {
            SoundName = reader.ReadString();
            Coordinates = Position.FromReaderInt(reader);
            Volume = reader.ReadFloat();
            Pitch = reader.ReadByte();
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(ID);
            stream.WriteString(SoundName);
            Coordinates.ToStreamInt(ref stream);
            stream.WriteFloat(Volume);
            stream.WriteByte(Pitch);
            stream.Purge();
        }
    }
}