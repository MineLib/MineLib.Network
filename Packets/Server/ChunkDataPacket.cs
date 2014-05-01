using MineLib.Network.IO;
using MineLib.Network.Data;

namespace MineLib.Network.Packets.Server
{
    public struct ChunkDataPacket : IPacket
    {
        public Coordinates2D Coordinates;
        public bool GroundUp;
        public ushort PrimaryBitMap;
        public ushort AddBitMap;
        public byte[] Data; // Maybe NbtByteArray?
        public bool SkyLightSend;

        // -- Debugging
        public int[] PrimaryBitMapConverted { get { return Converter.ConvertUShort(PrimaryBitMap); } }
        public int[] AddBitMapConverted { get { return Converter.ConvertUShort(AddBitMap); } }
        // -- Debugging

        public const byte PacketID = 0x21;
        public byte Id { get { return PacketID; } }

        public void ReadPacket(PacketByteReader stream)
        {
            Coordinates.X = stream.ReadInt();
            Coordinates.Z = stream.ReadInt();
            GroundUp = stream.ReadBoolean();
            SkyLightSend = true; // Assumed true in 0x21
            PrimaryBitMap = stream.ReadUShort();
            AddBitMap = stream.ReadUShort();
            var length = stream.ReadInt(); // was short.
            Data = stream.ReadByteArray(length);
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(Id);
            stream.WriteInt(Coordinates.X);
            stream.WriteInt(Coordinates.Z);
            stream.WriteBool(GroundUp);
            stream.WriteUShort(PrimaryBitMap);
            stream.WriteUShort(AddBitMap);
            stream.WriteVarInt(Data.Length);
            stream.WriteByteArray(Data);
            stream.Purge();
        }
    }
}