using MineLib.Network.Data;
using MineLib.Network.Enums;
using MineLib.Network.IO;

namespace MineLib.Network.Packets.Server
{
    public struct PlayerPositionAndLookPacket : IPacket
    {
        public Vector3 Vector3;
        public float Yaw, Pitch;
        public PlayerPositionAndLookFlags Flags;

        public const byte PacketID = 0x08;
        public byte Id { get { return PacketID; } }

        public void ReadPacket(PacketByteReader reader)
        {
            Vector3 = Vector3.FromReaderDouble(reader);
            Yaw = reader.ReadFloat();
            Pitch = reader.ReadFloat();
            Flags = (PlayerPositionAndLookFlags) reader.ReadSByte();
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(Id);
            Vector3.ToStreamDouble(ref stream);
            stream.WriteFloat(Yaw);
            stream.WriteFloat(Pitch);
            stream.WriteSByte((sbyte) Flags);
            stream.Purge();
        }
    }
}