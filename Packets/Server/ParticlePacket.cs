using CWrapped;

namespace MineLib.Network.Packets.Server
{
    public struct ParticlePacket : IPacket
    {
        public string ParticleName;
        public float X, Y, Z;
        public float OffsetX, OffsetY, OffsetZ;
        public float ParticleData;
        public int NumberOfParticles;

        public const byte PacketID = 0x2A;
        public byte Id { get { return PacketID; } }

        public void ReadPacket(ref Wrapped stream)
        {
            ParticleName = stream.ReadString();
            X = stream.ReadFloat();
            Y = stream.ReadFloat();
            Z = stream.ReadFloat();
            OffsetX = stream.ReadFloat();
            OffsetY = stream.ReadFloat();
            OffsetZ = stream.ReadFloat();
            ParticleData = stream.ReadFloat();
            NumberOfParticles = stream.ReadInt();
        }

        public void WritePacket(ref Wrapped stream)
        {
            stream.WriteVarInt(Id);
            stream.WriteString(ParticleName);
            stream.WriteFloat(X);
            stream.WriteFloat(Y);
            stream.WriteFloat(Z);
            stream.WriteFloat(OffsetX);
            stream.WriteFloat(OffsetY);
            stream.WriteFloat(OffsetZ);
            stream.WriteFloat(ParticleData);
            stream.WriteInt(NumberOfParticles);
        }
    }
}