using MineLib.Network.Data;
using MineLib.Network.Enums;
using MineLib.Network.IO;

namespace MineLib.Network.Packets.Server
{
    public struct EffectPacket : IPacket
    {
        public EffectID EffectID;
        public Position Location;
        public int Data;
        public bool DisableRelativeVolume;

        public byte ID { get { return 0x28; } }

        public void ReadPacket(PacketByteReader reader)
        {
            EffectID = (EffectID) reader.ReadInt();
            Location = Position.FromReaderLong(reader);
            Data = reader.ReadInt();
            DisableRelativeVolume = reader.ReadBoolean();
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(ID);
            stream.WriteInt((int) EffectID);
            Location.ToStreamLong(ref stream);
            stream.WriteInt(Data);
            stream.WriteBoolean(DisableRelativeVolume);
            stream.Purge();
        }
    }
}