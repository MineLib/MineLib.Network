using CWrapped;
using MineLib.Network.Data;

namespace MineLib.Network.Packets.Server
{
    public struct ChunkDataPacket : IPacket
    {
        public Vector3 Coordinates;
        public bool GroundUpContinuous;
        public short PrimaryBitMap;
        public short AddBitMap;
        public byte[] Data; // Maybe NbtByteArray?
        public byte[] Trim;

        public const byte PacketId = 0x21;
        public byte Id { get { return 0x21; } }

        public void ReadPacket(ref Wrapped stream)
        {
            Coordinates.X = stream.ReadInt();
            Coordinates.Z = stream.ReadInt();
            GroundUpContinuous = stream.ReadBool();
            PrimaryBitMap = stream.ReadShort();
            AddBitMap = stream.ReadShort();
            var length = stream.ReadInt(); // was short.
            Data = stream.ReadByteArray(length);

            Trim = new byte[length - 2];  
        }

        public void WritePacket(ref Wrapped stream)
        {
            stream.WriteVarInt(Id);
            stream.WriteInt((int)Coordinates.X);
            stream.WriteInt((int)Coordinates.Z);
            stream.WriteBool(GroundUpContinuous);
            stream.WriteShort(PrimaryBitMap);
            stream.WriteShort(AddBitMap);
            stream.WriteVarInt(Data.Length);
            stream.WriteByteArray(Data);
            stream.Purge();
        }
    }
}