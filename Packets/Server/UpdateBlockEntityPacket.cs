using MineLib.Network.Data;
using MineLib.Network.IO;

namespace MineLib.Network.Packets.Server
{
    public struct UpdateBlockEntityPacket : IPacket
    {
        public Vector3 Vector3;
        public byte Action; // Convert
        public byte[] NBTData;

        public const byte PacketID = 0x35;
        public byte Id { get { return PacketID; } }

        public void ReadPacket(PacketByteReader stream)
        {
            Vector3.X = stream.ReadInt();
            Vector3.Y = stream.ReadShort();
            Vector3.Z = stream.ReadInt();
            Action = stream.ReadByte();
            int length = stream.ReadShort();

            if (length>0)
                NBTData = stream.ReadByteArray(length);
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(Id);
            stream.WriteInt((int)Vector3.X);
            stream.WriteShort((short)Vector3.Y);
            stream.WriteInt((int)Vector3.Z);
            stream.WriteByte(Action);
            stream.WriteShort((short)NBTData.Length);
            stream.WriteByteArray(NBTData);
            stream.Purge();
        }
    }
}
