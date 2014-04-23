using MineLib.Network.IO;
using MineLib.Network.Data;

namespace MineLib.Network.Packets.Server
{
    public struct ChunkDataPacket : IPacket
    {
        public Vector2 Vector2;
        public bool GroundUpContinuous;
        public short PrimaryBitMap;
        public short AddBitMap;
        public byte[] Data; // Maybe NbtByteArray?
        public byte[] Trim;

        public const byte PacketID = 0x21;
        public byte Id { get { return PacketID; } }

        public void ReadPacket(PacketByteReader stream)
        {
            Vector2.X = stream.ReadInt();
            Vector2.Z = stream.ReadInt();
            GroundUpContinuous = stream.ReadBool();
            PrimaryBitMap = stream.ReadShort();
            AddBitMap = stream.ReadShort();
            var length = stream.ReadInt(); // was short.
            Data = stream.ReadByteArray(length);

            Trim = new byte[length - 2];  
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(Id);
            stream.WriteInt((int)Vector2.X);
            stream.WriteInt((int)Vector2.Z);
            stream.WriteBool(GroundUpContinuous);
            stream.WriteShort(PrimaryBitMap);
            stream.WriteShort(AddBitMap);
            stream.WriteVarInt(Data.Length);
            stream.WriteByteArray(Data);
            stream.Purge();
        }
    }
}