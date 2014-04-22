using MineLib.Network.Data;
using MineLib.Network.IO;

namespace MineLib.Network.Packets.Server
{
    public struct EffectPacket : IPacket
    {
        public int EffectID;
        public Vector3 Vector3;
        public int Data;
        public bool DisableRelativeVolume;

        public const byte PacketID = 0x28;
        public byte Id { get { return PacketID; } }

        public void ReadPacket(PacketByteReader stream)
        {
            EffectID = stream.ReadInt();
            Vector3.X = stream.ReadInt();
            Vector3.Y = stream.ReadByte();
            Vector3.Z = stream.ReadInt();
            Data = stream.ReadInt();
            DisableRelativeVolume = stream.ReadBool();
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(Id);
            stream.WriteInt(EffectID);
            stream.WriteInt((int)Vector3.X);
            stream.WriteByte((byte)Vector3.Y);
            stream.WriteInt((int)Vector3.Z);
            stream.WriteInt(Data);
            stream.WriteBool(DisableRelativeVolume);
            stream.Purge();
        }
    }
}