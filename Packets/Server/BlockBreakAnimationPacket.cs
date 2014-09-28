using MineLib.Network.Data;
using MineLib.Network.IO;

namespace MineLib.Network.Packets.Server
{
    public struct BlockBreakAnimationPacket : IPacket
    {
        public int EntityID;
        public Position Location;
        public sbyte DestroyStage;

        public byte ID { get { return 0x25; } }

        public void ReadPacket(PacketByteReader reader)
        {
            EntityID = reader.ReadVarInt();
            Location = Position.FromReaderLong(reader);
            DestroyStage = reader.ReadSByte();
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(ID);
            stream.WriteVarInt(EntityID);
            Location.ToStreamLong(ref stream);
            stream.WriteSByte(DestroyStage);
            stream.Purge();
        }
    }
}