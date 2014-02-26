using CWrapped;
using MineLib.Network.Enums;

namespace MineLib.Network.Packets.Server
{
    public struct SpawnObjectPacket : IPacket
    {
        public int EntityID;
        public Objects Type;
        public int X, Y, Z;
        public int Data; // Maybe new data-type ObjectData?
        public short? SpeedX, SpeedY, SpeedZ;
        public byte Yaw, Pitch;

        public const byte PacketId = 0x0E;
        public byte Id { get { return 0x0E; } }

        public void ReadPacket(ref Wrapped stream)
        {
            EntityID = stream.ReadVarInt();
            Type = (Objects)stream.ReadByte();
            X = stream.ReadInt();
            Y = stream.ReadInt();
            Z = stream.ReadInt();
            Yaw = stream.ReadByte();
            Pitch = stream.ReadByte();
            Data = stream.ReadInt();
            if (Data != 0)
            {
                SpeedX = stream.ReadShort();
                SpeedY = stream.ReadShort();
                SpeedZ = stream.ReadShort();
            }
        }

        public void WritePacket(ref Wrapped stream)
        {
            stream.WriteVarInt(Id);
            stream.WriteVarInt(EntityID);
            stream.WriteByte((byte)Type);
            stream.WriteInt(X);
            stream.WriteInt(Y);
            stream.WriteInt(Z);
            stream.WriteByte(Yaw);
            stream.WriteByte(Pitch);
            stream.WriteInt(Data);
            if (Data != 0)
            {
                stream.WriteShort(SpeedX.Value);
                stream.WriteShort(SpeedY.Value);
                stream.WriteShort(SpeedZ.Value);
            }
            stream.Purge();
        }
    }
}