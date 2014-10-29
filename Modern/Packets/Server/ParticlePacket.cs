using MineLib.Network.IO;
using MineLib.Network.Modern.Enums;

namespace MineLib.Network.Modern.Packets.Server
{
    public struct ParticlePacket : IPacket
    {
        public Particle ParticleID;
        public bool LongDistance;
        public float X, Y, Z;
        public float OffsetX, OffsetY, OffsetZ;
        public float ParticleData;
        public int NumberOfParticles;
        public int[] Data;

        public byte ID { get { return 0x2A; } }

        public IPacket ReadPacket(IMinecraftDataReader reader)
        {
            ParticleID = (Particle) reader.ReadInt();
            LongDistance = reader.ReadBoolean();
            X = reader.ReadFloat();
            Y = reader.ReadFloat();
            Z = reader.ReadFloat();
            OffsetX = reader.ReadFloat();
            OffsetY = reader.ReadFloat();
            OffsetZ = reader.ReadFloat();
            ParticleData = reader.ReadFloat();
            NumberOfParticles = reader.ReadInt();

            switch (ParticleID)
            {
                case Particle.ITEM_CRACK:
                case Particle.BLOCK_CRACK:
                //case Particle.BLOCK_DUST:
                    Data = reader.ReadVarIntArray(2);
                    break;

                default:
                    Data = reader.ReadVarIntArray(0);
                    break;
            }

            return this;
        }

        public IPacket WritePacket(IMinecraftStream stream)
        {
            stream.WriteVarInt(ID);
            stream.WriteInt((int) ParticleID);
            stream.WriteBoolean(LongDistance);
            stream.WriteFloat(X);
            stream.WriteFloat(Y);
            stream.WriteFloat(Z);
            stream.WriteFloat(OffsetX);
            stream.WriteFloat(OffsetY);
            stream.WriteFloat(OffsetZ);
            stream.WriteFloat(ParticleData);
            stream.WriteInt(NumberOfParticles);
            stream.WriteVarIntArray(Data);

            return this;
        }
    }
}