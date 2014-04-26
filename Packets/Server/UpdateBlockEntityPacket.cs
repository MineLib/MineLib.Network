using MineLib.Network.Data;
using MineLib.Network.IO;

namespace MineLib.Network.Packets.Server
{
    public struct UpdateBlockEntityPacket : IPacket
    {
        public Coordinates3D Coordinates;
        public byte Action; // Convert
        public byte[] NBTData;

        public const byte PacketID = 0x35;
        public byte Id { get { return PacketID; } }

        public void ReadPacket(PacketByteReader stream)
        {
            Coordinates.X = stream.ReadInt();
            Coordinates.Y = stream.ReadShort();
            Coordinates.Z = stream.ReadInt();
            Action = stream.ReadByte();
            int length = stream.ReadShort();

            if (length>0)
                NBTData = stream.ReadByteArray(length);
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(Id);
            stream.WriteInt(Coordinates.X);
            stream.WriteShort((short)Coordinates.Y);
            stream.WriteInt(Coordinates.Z);
            stream.WriteByte(Action);
            stream.WriteShort((short)NBTData.Length);
            stream.WriteByteArray(NBTData);
            stream.Purge();
        }
    }
}
