using MineLib.Network.Data;
using MineLib.Network.IO;

namespace MineLib.Network.Packets.Server
{
    public struct SpawnPositionPacket : IPacket
    {
        public Vector3 Vector3;

        public const byte PacketID = 0x05;
        public byte Id { get { return PacketID; } }

        public void ReadPacket(PacketByteReader stream)
        {
            Vector3.X = stream.ReadInt();
            Vector3.Y = stream.ReadInt();
            Vector3.Z = stream.ReadInt();
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(Id);
            stream.WriteInt((int)Vector3.X);
            stream.WriteInt((int)Vector3.Y);
            stream.WriteInt((int)Vector3.Z);
            stream.Purge();
        }
    }
}