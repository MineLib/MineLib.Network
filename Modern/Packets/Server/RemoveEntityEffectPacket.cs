using MineLib.Network.IO;
using MineLib.Network.Modern.Enums;

namespace MineLib.Network.Modern.Packets.Server
{
    public struct RemoveEntityEffectPacket : IPacket
    {
        public int EntityID;
        public EffectID EffectID;

        public byte ID { get { return 0x1E; } }

        public IPacket ReadPacket(IMinecraftDataReader reader)
        {
            EntityID = reader.ReadVarInt();
            EffectID = (EffectID) reader.ReadSByte();

            return this;
        }

        public IPacket WritePacket(IMinecraftStream stream)
        {
            stream.WriteVarInt(ID);
            stream.WriteVarInt(EntityID);
            stream.WriteSByte((sbyte) EffectID);
            stream.Purge();

            return this;
        }
    }
}