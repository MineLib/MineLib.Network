using MineLib.Network.Data;
using MineLib.Network.Enums;
using MineLib.Network.IO;

namespace MineLib.Network.Packets.Server
{
    public struct EffectPacket : IPacket
    {
        public EffectID EffectID;
        public Coordinates3D Coordinates;
        public int Data;
        public bool DisableRelativeVolume;

        public const byte PacketID = 0x28;
        public byte Id { get { return PacketID; } }

        public void ReadPacket(PacketByteReader stream)
        {
            EffectID = (EffectID)stream.ReadInt();
            Coordinates.X = stream.ReadInt();
            Coordinates.Y = stream.ReadByte();
            Coordinates.Z = stream.ReadInt();
            Data = stream.ReadInt();
            DisableRelativeVolume = stream.ReadBool();
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(Id);
            stream.WriteInt((int)EffectID);
            stream.WriteInt(Coordinates.X);
            stream.WriteByte((byte)Coordinates.Y);
            stream.WriteInt(Coordinates.Z);
            stream.WriteInt(Data);
            stream.WriteBool(DisableRelativeVolume);
            stream.Purge();
        }
    }
}