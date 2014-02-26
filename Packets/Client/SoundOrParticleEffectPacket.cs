using CWrapped;

namespace MineLib.Network.Packets.Client
{
    public struct SoundOrParticleEffectPacket : IPacket
    {
        public int EntityID;
        public int X;
        public byte Y;
        public int Z;
        public int Data;
        public bool DisableRelativeVolume;

        public const byte PacketId = 0x3D;
        public byte Id { get { return 0x3D; } }

        public void ReadPacket(ref Wrapped stream)
        {
            EntityID = stream.ReadShort();
            X = stream.ReadShort();
            Y = stream.ReadByte();
            Z = stream.ReadShort();
            Data = stream.ReadShort();
            DisableRelativeVolume = stream.ReadBool();
        }

        public void WritePacket(ref Wrapped stream)
        {
            stream.WriteVarInt(Id);
            stream.WriteVarInt(EntityID);
            stream.WriteVarInt(X);
            stream.WriteVarInt(Y);
            stream.WriteVarInt(Z);
            stream.WriteVarInt(Data);
            stream.WriteBool(DisableRelativeVolume);
            stream.Purge();
        }
    }
}