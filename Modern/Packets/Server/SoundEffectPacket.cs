using MineLib.Network.IO;
using MineLib.Network.Modern.Data;

namespace MineLib.Network.Modern.Packets.Server
{
    public struct SoundEffectPacket : IPacket
    {
        public string SoundName;
        public Position Coordinates;
        public float Volume;
        public byte Pitch;

        public byte ID { get { return 0x29; } }

        public IPacket ReadPacket(MinecraftDataReader reader)
        {
            SoundName = reader.ReadString();
            Coordinates = Position.FromReaderInt(reader);
            Volume = reader.ReadFloat();
            Pitch = reader.ReadByte();

            return this;
        }

        public IPacket WritePacket(MinecraftStream stream)
        {
            stream.WriteVarInt(ID);
            stream.WriteString(SoundName);
            Coordinates.ToStreamInt(stream);
            stream.WriteFloat(Volume);
            stream.WriteByte(Pitch);
            stream.Purge();

            return this;
        }
    }
}