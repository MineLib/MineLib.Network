using CWrapped;
using MineLib.Network.Data.EntityMetadata;

namespace MineLib.Network.Packets.Server
{
    public struct SpawnPlayerPacket : IPacket
    {
        public int EntityID;
        public string PlayerUUID, PlayerName;
        public int X, Y, Z;
        public byte Yaw, Pitch;
        public short CurrentItem;
        public MetadataDictionary Metadata;

        public const byte PacketID = 0x0C;
        public byte Id { get { return PacketID; } }
    
        public void ReadPacket(ref Wrapped stream)
        {
            EntityID = stream.ReadVarInt();
            PlayerUUID = stream.ReadString();
            PlayerName = stream.ReadString();
            X = stream.ReadInt();
            Y = stream.ReadInt();
            Z = stream.ReadInt();
            Yaw = stream.ReadByte();
            Pitch = stream.ReadByte();
            CurrentItem = stream.ReadShort();
            Metadata = MetadataDictionary.FromStream(ref stream);

        }
    
        public void WritePacket(ref Wrapped stream)
        {
            stream.WriteVarInt(Id);
            stream.WriteVarInt(EntityID);
            stream.WriteString(PlayerUUID);
            stream.WriteString(PlayerName);
            stream.WriteInt(X);
            stream.WriteInt(Y);
            stream.WriteInt(Z);
            stream.WriteByte(Yaw);
            stream.WriteByte(Pitch);
            stream.WriteShort(CurrentItem);
            Metadata.WriteTo(ref stream);
            stream.Purge();
        }
    }
}