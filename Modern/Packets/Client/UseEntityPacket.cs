using MineLib.Network.IO;
using MineLib.Network.Modern.Enums;

namespace MineLib.Network.Modern.Packets.Client
{
    public struct UseEntityPacket : IPacket
    {
        public int Target;
        public UseEntity Type;
        public float TargetX, TargetY, TargetZ;

        public byte ID { get { return 0x02; } }

        public IPacket ReadPacket(MinecraftDataReader reader)
        {
            Target = reader.ReadVarInt();
            Type = (UseEntity) reader.ReadVarInt();

            if (Type == UseEntity.INTERACT_AT)
            {
                TargetX = reader.ReadFloat();
                TargetY = reader.ReadFloat();
                TargetZ = reader.ReadFloat();
            }

            return this;
        }

        public IPacket WritePacket(MinecraftStream stream)
        {
            stream.WriteVarInt(ID);
            stream.WriteVarInt(Target);
            stream.WriteVarInt((byte) Type);

            if (Type == UseEntity.INTERACT_AT)
            {
                stream.WriteFloat(TargetX);
                stream.WriteFloat(TargetY);
                stream.WriteFloat(TargetZ);
            }

            stream.Purge();

            return this;
        }
    }
}