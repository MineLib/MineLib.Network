using MineLib.Network.IO;
using MineLib.Network.Data;

namespace MineLib.Network.Packets.Server
{
    public struct ChunkDataPacket : IPacket
    {
        public Coordinates2D Coordinates;
        public bool GroundUp;
        public ushort PrimaryBitMap;
        public byte[] Data;

        // -- Debugging
        public int[] PrimaryBitMapConverted { get { return Converter.ConvertUShort(PrimaryBitMap); } }
        // -- Debugging

        public const byte PacketID = 0x21;
        public byte Id { get { return PacketID; } }

        public void ReadPacket(PacketByteReader reader)
        {
            Coordinates.X = reader.ReadInt();
            Coordinates.Z = reader.ReadInt();
            GroundUp = reader.ReadBoolean();
            PrimaryBitMap = reader.ReadUShort();
            int size = reader.ReadVarInt();
            Data = reader.ReadByteArray(size);
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(Id);
            stream.WriteInt(Coordinates.X);
            stream.WriteInt(Coordinates.Z);
            stream.WriteBoolean(GroundUp);
            stream.WriteUShort(PrimaryBitMap);
            stream.WriteVarInt(Data.Length);
            stream.WriteByteArray(Data);
            stream.Purge();
        }
    }
}