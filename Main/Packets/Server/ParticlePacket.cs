using MineLib.Network.IO;
using MineLib.Network.Main.Enums;

namespace MineLib.Network.Main.Packets.Server
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

        public void ReadPacket(PacketByteReader reader)
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
        }

        public void WritePacket(ref PacketStream stream)
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
        }
    }
}