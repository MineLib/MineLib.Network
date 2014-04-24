using MineLib.Network.Data;
using MineLib.Network.IO;
using MineLib.Network.Data.EntityMetadata;

namespace MineLib.Network.Packets.Server
{
    public struct SpawnPlayerData
    {
        public string Name;
        public string Value;
        public string Signature;
    }

    public struct SpawnPlayerPacket : IPacket
    {
        public int EntityID;
        public string PlayerUUID, PlayerName;
        public SpawnPlayerData[] Data;
        public Vector3 Vector3;
        public byte Yaw, Pitch;
        public short CurrentItem;
        public MetadataDictionary Metadata;

        public const byte PacketID = 0x0C;
        public byte Id { get { return PacketID; } }
    
        public void ReadPacket(PacketByteReader stream)
        {
            EntityID = stream.ReadVarInt();
            PlayerUUID = stream.ReadString();
            PlayerName = stream.ReadString();

            int length = stream.ReadVarInt();
            Data = new SpawnPlayerData[length];
            for (int i = 0; i < length; i++)
            {
                Data[i].Name = stream.ReadString();
                Data[i].Value = stream.ReadString();
                Data[i].Signature = stream.ReadString();
            }

            Vector3.X = stream.ReadInt() / 32;
            Vector3.Y = stream.ReadInt() / 32;
            Vector3.Z = stream.ReadInt() / 32;
            Yaw = stream.ReadByte();
            Pitch = stream.ReadByte();
            CurrentItem = stream.ReadShort();
            Metadata = MetadataDictionary.FromStream(stream);

        }
    
        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(Id);
            stream.WriteVarInt(EntityID);
            stream.WriteString(PlayerUUID);
            stream.WriteString(PlayerName);
            stream.WriteInt((int)Vector3.X * 32);
            stream.WriteInt((int)Vector3.Y * 32);
            stream.WriteInt((int)Vector3.Z * 32);
            stream.WriteByte(Yaw);
            stream.WriteByte(Pitch);
            stream.WriteShort(CurrentItem);
            Metadata.WriteTo(ref stream);
            stream.Purge();
        }
    }
}