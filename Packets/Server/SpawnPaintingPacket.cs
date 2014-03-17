using MineLib.Network.IO;


namespace MineLib.Network.Packets.Server
{
    public struct SpawnPaintingPacket : IPacket
    {
        public int EntityID;
        public string Title;
        public int X, Y, Z;
        public int Direction;

        public const byte PacketID = 0x10;
        public byte Id { get { return PacketID; } }

        public void ReadPacket(ref Wrapped stream)
        {
            EntityID = stream.ReadInt();
            Title = stream.ReadString();
            X = stream.ReadInt();
            Y = stream.ReadInt();
            Z = stream.ReadInt();
            Direction = stream.ReadInt();
        }

        public void WritePacket(ref Wrapped stream)
        {
            stream.WriteVarInt(Id);
            stream.WriteInt(EntityID);
            stream.WriteString(Title);
            stream.WriteInt(X);
            stream.WriteInt(Y);
            stream.WriteInt(Z);
            stream.WriteInt(Direction);
            stream.Purge();
        }
    }
}