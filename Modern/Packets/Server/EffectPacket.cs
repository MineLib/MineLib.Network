using MineLib.Network.Data;
using MineLib.Network.IO;
using MineLib.Network.Modern.Enums;

namespace MineLib.Network.Modern.Packets.Server
{
    public struct EffectPacket : IPacket
    {
        public EffectID EffectID;
        public Position Location;
        public int Data;
        public bool DisableRelativeVolume;

        public byte ID { get { return 0x28; } }

        public IPacket ReadPacket(IMinecraftDataReader reader)
        {
            EffectID = (EffectID) reader.ReadInt();
            Location = Position.FromReaderLong(reader);
            Data = reader.ReadInt();
            DisableRelativeVolume = reader.ReadBoolean();

            return this;
        }

        public IPacket WritePacket(IMinecraftStream stream)
        {
            stream.WriteVarInt(ID);
            stream.WriteInt((int) EffectID);
            Location.ToStreamLong(stream);
            stream.WriteInt(Data);
            stream.WriteBoolean(DisableRelativeVolume);
            stream.Purge();

            return this;
        }
    }
}