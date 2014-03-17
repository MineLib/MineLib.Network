using MineLib.Network.IO;


namespace MineLib.Network.Packets.Server
{
    public struct UpdateBlockEntityPacket : IPacket
    {
        public int X;
        public short Y;
        public int Z;
        public byte Action; // Convert
        public byte[] NBTData;

        public const byte PacketID = 0x35;
        public byte Id { get { return PacketID; } }

        public void ReadPacket(ref Wrapped stream)
        {
            X = stream.ReadInt();
            Y = stream.ReadShort();
            Z = stream.ReadInt();
            Action = stream.ReadByte();
            int length = stream.ReadShort();

            if (length>0)
                NBTData = stream.ReadByteArray(length);
        }

        public void WritePacket(ref Wrapped stream)
        {
            stream.WriteVarInt(Id);
            stream.WriteInt(X);
            stream.WriteShort(Y);
            stream.WriteInt(Z);
            stream.WriteByte(Action);
            stream.WriteShort((short)NBTData.Length);
            stream.WriteByteArray(NBTData);
            stream.Purge();
        }
    }
}
